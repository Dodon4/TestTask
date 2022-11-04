using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TestSerializer
{
    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }


    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            Dictionary<ListNode, int> dictionary = new Dictionary<ListNode, int>();
            int id = 0;
            for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
            {
                dictionary.Add(currentNode, id);
                id++;
            }
            using (BinaryWriter writer = new BinaryWriter(s))
            {
                for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
                {
                    writer.Write(currentNode.Data);
                    writer.Write(dictionary[currentNode.Rand]);
                }
            }

        }

        public void Deserialize(FileStream s)
        {
            Dictionary<int, Tuple<String, int>> dictionary = new Dictionary<int, Tuple<String, int>>();
            int counter = 0;
            using (BinaryReader reader = new BinaryReader(s))
            {
                while (reader.PeekChar() != -1)
                {
                    String data = reader.ReadString();
                    int randomId = reader.ReadInt32();
                    dictionary.Add(counter, new Tuple<String, int>(data, randomId));
                    counter++;
                }
            }
            Count = counter;
            Head = new ListNode();
            ListNode current = Head;
            ListNode[] listNodes = new ListNode[Count];
            for (int i = 0; i < Count; i++)
            {
                current.Data = dictionary.ElementAt(i).Value.Item1;
                current.Next = new ListNode();
                if (i != this.Count - 1)
                {
                    current.Next.Prev = current;
                    current = current.Next;
                }
                else
                {
                    Tail = current;
                }
                listNodes[i] = current;
            }
            counter = 0;
            for (ListNode currentNode = Head; currentNode.Next != null; currentNode = currentNode.Next)
            {
                currentNode.Rand = listNodes[(dictionary.ElementAt(counter).Value.Item2)];
                counter++;
            }
        }
    }
}
