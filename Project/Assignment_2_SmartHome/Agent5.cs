using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome {
    /// <summary>
    /// decides what it learnsfrom Qlearning
    /// solar panels are added here and theiir respective functions
    /// Q-learning is performed with negotiation functino.
    /// </summary>
    public class Agent5 {

        private Agent1 ag1_;
        private Agent2 ag2_;
        private Agent3 ag3_;

        private SmartHome smartHome_;
        private double[,] qMatrix_;
        private double slack_ = 1.36;

        private bool state_, goalFound_;
        private double gamma_ = 0.5;
        private int width_ = 3, height_ = 7;
        private int hour_;

        private List<Node> graph_;
        private List<Node> listOfGoals_;

        public Agent5(SmartHome smarthome) {
            smartHome_ = smarthome;
            ag1_ = smartHome_.getAgent1();
            ag2_ = smartHome_.getAgent2();
            ag3_ = smartHome_.getAgent3();

            graph_ = new List<Node>();
            listOfGoals_ = new List<Node>();
            qMatrix_ = new double[height_, width_];
            generateGraph();
            matricesQ();
        }
        //gets hour
        public void setHour(int hour) {
            hour_ = hour;
        }
        //used to get signal from agent 4
        public bool stateIs(bool state) {
            return state_ = state; ;
        }
        //sets up the solar panel
        public double solarPanel(int hour) {
            double peakPower_ = 0.25;
            //R
            double[] cloudLevel = { 1.0,   // clear skies
                                    0.7,   // lightly clouded
                                    0.5,   // partly clouded
                                    0.3,   // cloudy
                                    0.1 }; //rain/snow
            Random randy = new Random();
            double sunshineLevel = randy.Next(0, cloudLevel.Count());
            double energyGeneration = (hour > 6 && hour < 19) ? (20 * peakPower_ * sunshineLevel) / 24 : 0;
            return energyGeneration;
        }
        public double getBuyNOK(int hour) {
            double buy = (hour >= 6 && hour <= 24) ? 1.2 : 0.8;
            return buy;
        }
        public double getSellNOK(int hour) {
            double sell = (hour <= 0 && hour >= 6) ? 0.3 : 0.0;
            return sell;
        }
        //is the negotiator between agent 5 and agent 1
        public double negotiateAG5AG1(double kW, double pricePerHour) {
            Random randy = new Random();

            double µAg5 = ag1_.consumptionHeatedFloorMin() * pricePerHour;
            double µAg1 = ag1_.consumptionHeatedFloorMax() * pricePerHour;

            double rand = randy.NextDouble();
            double sigmoidAg5 = 1 / (1 + Math.Pow(Math.E, ((kW - µAg5) / slack_))); // buy
            double sigmoidAg1 = 1 - (1 / ((1 + Math.Pow(Math.E, (kW - µAg5))) / ag1_.Slack)); // sell

            //if rand is greater than either of the sigmoids then agent5 and agent1 is in agreement
            if (rand > sigmoidAg5 && rand > sigmoidAg1) {
                return kW;
            }
            //if kW is greater than µAg1, then kW = µAg5 so that it starts all over again
            else if (kW > µAg1) {
                return kW = µAg5;
            }
            //
            else {
                slack_ += 0.36;
                //ag1_.Slack += 0.36;
            }

            return negotiateAG5AG1(kW + 1, pricePerHour);
        }
        //is the negotiator between agent 5 and agent 2
        public double negotiateAG5AG2(double kW, double pricePerHour) {
            Random randy = new Random();

            double µAg5 = ag2_.consumptionCenteralHeaterMin() * pricePerHour;
            double µAg2 = ag2_.consumptionCenteralHeaterMax() * pricePerHour;

            double rand = randy.NextDouble();

            double sigmoidAg5 = 1 / (1 + Math.Pow(Math.E, ((kW - µAg5) / slack_))); // buy
            double sigmoidAg2 = 1 - (1 / ((1 + Math.Pow(Math.E, (kW - µAg5))) / ag2_.Slack_)); // sell

            if (rand > sigmoidAg5 && rand > sigmoidAg2) {
                return kW;
            }
            //if kW is greater than µAg1 then they have not reached an agreement, then kW = µAg5 so that it starts all over again
            else if (kW > µAg2) {
                return kW = µAg5;
            }
            else {
                slack_ += 0.36;
                //ag2_.Slack += 0.36;
            }
            return negotiateAG5AG2(kW + 1, pricePerHour);
        }
        private Node getNodeById(int id) {
            foreach (var node in graph_) {
                if (node.Id == id) {
                    return node;
                }
            }
            return null;
        }
        private int concatenate(int i, int j) {
            if (i <= 0 || i > height_)
                return 00;
            if (j <= 0 || j > width_)
                return 00;

            int pow = 10;
            while (j >= pow)
                pow *= 10;
            return i * pow + j;
        }
        private void generateGraph() {
            for (int i = 1; i <= height_; i++) {
                for (int j = 1; j <= width_; j++) {
                    int id = concatenate(i, j);
                    int left = concatenate(i, j - 1);
                    int right = concatenate(i, j + 1);

                    var node = new Node(id, left, right, j - 1);
                    graph_.Add(node);

                    //adds the cells of the goal state to the listOfGoals
                    if (j == 1)
                        listOfGoals_.Add(node);
                }
            }
        }
        //sets up matrix
        private void matricesQ() {
            for (int i = 0; i < height_; i++) {
                for (int j = 0; j < width_; j++) {
                    qMatrix_[i, j] = 0;
                }
            }
        }
        private int getState(Node nextState) {
            int state = nextState.getState();
            return state;
        }
        //get the maximum q-value of future actions
        private double maxQ(Node nextAction) {
            List<double> actions = new List<double>();
            int currentState = getState(nextAction);
            for (int i = 0; i < height_; i++) {
                actions.Add(qMatrix_[i, currentState]);
            }
            return actions.Max();
        }
        //populates the q-matrix with values gotten from actions randomly chosen
        public void fillingQMatrix(int hour) {
            goalFound_ = false;

            Random randy = new Random((int)DateTime.Now.Ticks);
            while (!goalFound_) {
                //select a random starting state
                int a = randy.Next(1, height_ + 1);
                int b = randy.Next(1, width_ + 1);
                Node currentNode = getNodeById(concatenate(a, b));

                //choose a random action
                int actionState = currentNode.Children[randy.Next(0, currentNode.Children.Count)];
                int state = getState(currentNode);
                int action = randy.Next(0, height_);
                double reward = 0;

                #region ACTION 0 ------ buy the energy
                //buy
                if (action == 0) {
                    if (currentNode.getState() == 0) {
                        double load = ag1_.calculateTheLoad() + ag2_.calculateTheLoad();
                        double production = solarPanel(hour);
                        double consumption = (production - load) * getBuyNOK(hour);
                        reward = consumption;
                    }
                    else if (currentNode.getState() == 1) {
                        reward = -50;
                    }
                    else if (currentNode.getState() == 2) {
                        reward = -100;
                    }
                }
                #endregion
                #region ACTION 1 ------ discharge the battery
                //discharge
                else if (action == 1) {

                    if (currentNode.getState() == 0) {
                        reward = -5;
                    }
                    else if (currentNode.getState() == 1) {
                        reward = -50;
                    }
                    else if (currentNode.getState() == 2) {
                        reward = -100;
                    }
                }
                #endregion
                #region ACTION 2 ------ reduce the load/temp
                //reduce load
                else if (action == 2) {

                    if (currentNode.getState() == 0) {

                        double µAg5_2 = ag2_.consumptionCenteralHeaterMin() * getBuyNOK(hour);
                        double µAg5_1 = ag1_.consumptionHeatedFloorMin() * getBuyNOK(hour);
                        double sumOfµ = µAg5_1 + µAg5_2;
                        double negotiationReward = negotiateAG5AG1(µAg5_1, getBuyNOK(hour)) + negotiateAG5AG2(µAg5_2, getBuyNOK(hour));

                        reward = sumOfµ - negotiationReward;
                    }
                    else if (currentNode.getState() == 1) {
                        reward = -50;
                    }
                    else if (currentNode.getState() == 2) {
                        reward = -100;
                    }
                }
                #endregion
                #region ACTION 3 ------ sell the energy
                //sell
                else if (action == 3) {

                    if (currentNode.getState() == 2) {
                        //double load = ag1.calculateTheLoad() + ag2.calculateTheLoad();
                        //double production = solarPanel(hour);
                        //double consumption = (production - load) * getBuyNOK(hour);
                        //
                        //reward = consumption;
                        reward = 3; // 0.3 NOK * 10 credits = 3 KW/h  
                    }
                    else if (currentNode.getState() == 1) {
                        reward = -50;
                    }
                    else if (currentNode.getState() == 0) {
                        reward = -100;
                    }
                }
                #endregion
                #region ACTION 4 ------ charge the battery
                //charge
                else if (action == 4) {
                    if (currentNode.getState() == 2) {
                        reward = 5;
                    }
                    else if (currentNode.getState() == 1) {
                        reward = -50;
                    }
                    else if (currentNode.getState() == 0) {
                        reward = -100;
                    }
                }
                #endregion
                #region ACTION 5------ increase the load/temp
                //increase load
                else if (action == 5) {
                    if (currentNode.getState() == 2) {
                        double µAg5_2 = ag2_.consumptionCenteralHeaterMax() * getBuyNOK(hour);
                        double µAg5_1 = ag1_.consumptionHeatedFloorMax() * getBuyNOK(hour);
                        double sumOfµ = µAg5_1 + µAg5_2;
                        double negotiationReward = negotiateAG5AG1(µAg5_1, getBuyNOK(hour)) + negotiateAG5AG2(µAg5_2, getBuyNOK(hour));

                        reward = sumOfµ - negotiationReward;
                    }
                    else if (currentNode.getState() == 1) {
                        reward = -50;
                    }
                    else if (currentNode.getState() == 0) {
                        reward = -100;
                    }
                }
                #endregion
                #region ACTION 6 ------ stay
                //stay in state
                else if (action == 6) {
                    if (currentNode.getState() == 2) {
                        reward = -50;
                    }
                    else if (currentNode.getState() == 1) {
                        reward = 100;
                    }
                    else if (currentNode.getState() == 0) {
                        reward = -50;
                    }
                }
                #endregion

                //peek at next state
                Node nextNode = getNodeById(actionState);
                double maxq = maxQ(nextNode); //maximun q value for next state based on all possible actions
                qMatrix_[action, state] = (int)(reward + gamma_ * maxq);
                currentNode = nextNode; // Set the next state as the current state
                if (listOfGoals_.Contains(currentNode)) { //has the goal state been reached?
                    goalFound_ = true;
                }
            }
        }
        public void printQMatrix() {
            Debug.WriteLine(" ");
            Debug.WriteLine("Q-matrix: ");
            for (int i = 0; i < height_; i++) {
                for (int j = 0; j < width_; j++) {
                    Debug.Write(Math.Round(qMatrix_[i, j], 2) + " \t\t");
                }
                Debug.WriteLine(" ");
            }
        }
    }
}
