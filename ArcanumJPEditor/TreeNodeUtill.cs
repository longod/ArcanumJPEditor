// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public class TreeNodeUtill {
        public delegate TreeNode GetNode( TreeNode node );
        static public TreeNode GetNext( TreeNode node ) {
            if ( node.Nodes.Count > 0 ) {
                return node.Nodes[ 0 ];
            }
            while ( node != null ) {
                if ( node.NextNode != null ) {
                    return node.NextNode;
                }
                node = node.Parent;
            }
            return null;
        }
        static public TreeNode GetPrev( TreeNode node ) {
            if ( node.Nodes.Count > 0 ) {
                return node.Nodes[ node.Nodes.Count - 1 ];
            }
            while ( node != null ) {
                if ( node.PrevNode != null ) {
                    return node.PrevNode;
                }
                node = node.Parent;
            }
            return null;
        }
    }
}
