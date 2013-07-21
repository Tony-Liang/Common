using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Arithmetic
{
    public class TernaryTreecs
    {
        private Node m_root = null;

        private void Add(string s, int pos, ref Node node)
        {
            if (node == null) { node = new Node(s[pos], false); }

            if (s[pos] < node.m_char) { Add(s, pos, ref node.m_left); }
            else if (s[pos] > node.m_char) { Add(s, pos, ref node.m_right); }
            else
            {
                if (pos + 1 == s.Length) { node.m_wordEnd = true; }
                else { Add(s, pos + 1, ref node.m_center); }
            }
        }

        public void Add(string s)
        {
            if (s == null || s == "") throw new ArgumentException();

            Add(s, 0, ref m_root);
        }

        public bool Contains(string s)
        {
            if (s == null || s == "") throw new ArgumentException();

            int pos = 0;
            Node node = m_root;
            while (node != null)
            {
                int cmp = s[pos] - node.m_char;
                if (s[pos] < node.m_char) { node = node.m_left; }
                else if (s[pos] > node.m_char) { node = node.m_right; }
                else
                {
                    if (++pos == s.Length) return node.m_wordEnd;
                    node = node.m_center;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the world by dfs.
        /// </summary>
        /// <param name="prefix">The prefix of world.</param>
        /// <param name="node">The tree node.</param>
        private void DFS(string prefix, Node node)
        {
            if (node != null)
            {
                if (true == node.m_wordEnd)
                {
                    _hashSet.Add(prefix + node.Word);
                }

                DFS(prefix, node.LeftChild);
                DFS(prefix + node.Word, node.CenterChild);
                DFS(prefix, node.RightChild);
            }
        }
        ////private string _prefix;

        private HashSet<string> _hashSet;
        /// <summary>
        /// Finds the similar world.
        /// </summary>
        /// <param name="s">The prefix of the world.</param>
        /// <returns>The world has the same prefix.</returns>
        public HashSet<string> FindSimilar(string s)
        {
            Node node = this.Find(s);
            this.DFS(s, node);
            return _hashSet;
        }

        /// <summary>
        /// Finds the specified world.
        /// </summary>
        /// <param name="s">The specified world</param>
        /// <returns>The corresponding tree node.</returns>
        private Node Find(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException("s");
            }

            int pos = 0;
            Node node = m_root;
            _hashSet = new HashSet<string>();
            while (node != null)
            {
                if (s[pos] < node.Word)
                {
                    node = node.LeftChild;
                }
                else if (s[pos] > node.Word)
                {
                    node = node.RightChild;
                }
                else
                {
                    if (++pos == s.Length)
                    {
                        _hashSet.Add(s);
                        return node.CenterChild;
                    }

                    node = node.CenterChild;
                }
            }

            return null;
        }
    }

    internal class Node
    {
        internal char m_char;
        internal Node m_left, m_center, m_right;
        internal bool m_wordEnd;

        public Node(char ch, bool wordEnd)
        {
            m_char = ch;
            m_wordEnd = wordEnd;
        }

        public char Word
        {
            get
            {
                return m_char;
            }
        }

        public Node RightChild
        {
            get
            {
                return m_right;
            }
        }

        public Node LeftChild
        {
            get
            {
                return m_left;
            }
        }

        public Node CenterChild
        {
            get
            {
                return m_center;
            }
        }
    }
}
