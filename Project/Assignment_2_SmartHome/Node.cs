using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome {
    /// <summary>
    /// node class for q-learning
    /// </summary>
    public class Node : IComparable<Node> {

        private int left_, right_;
        private double iMax_ = Double.MaxValue;
        private int state_;

        public Node(int id, int left, int right, int state) {

            Id = id;
            state_ = state;
            left_ = left;
            right_ = right;

            Children = new List<int>();
            if (left != 00)
                Children.Add(left);
            if (right != 00)
                Children.Add(right);

            Q = iMax_; // Unknown distance from source to v
        }
        public int Id {
            get; set;
        }
        public override string ToString() {
            return Id.ToString();
        }
        public double Q {
            get; set;
        }
        public List<int> Children {
            get; set;
        }
        public int CompareTo(Node cmp) {
            return Q.CompareTo(cmp.Q);
        }
        public int getState() {
            return state_;
        }
    }
}
