using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Stos
{
    public class StosWTablicy<T> : IStos<T>, IEnumerable<T>
    {
        private T[] tab;
        private int szczyt = -1;
        public int Capacity => tab.Length;
        public T this[int index]{
            get{
                return tab[index];
            }
        }

        public StosWTablicy(int size = 10)
        {
            tab = new T[size];
            szczyt = -1;
        }

        public T Peek => IsEmpty ? throw new StosEmptyException() : tab[szczyt];

        public int Count => szczyt + 1;

        public bool IsEmpty => szczyt == -1;

        public void Clear() => szczyt = -1;

        public T Pop()
        {
            if (IsEmpty)
                throw new StosEmptyException();

            szczyt--;
            return tab[szczyt + 1];
        }

        public void Push(T value)
        {
            if (szczyt == tab.Length - 1)
            {
                Array.Resize(ref tab, tab.Length * 2);
            }

            szczyt++;
            tab[szczyt] = value;
        }

        public T[] ToArray()
        {
            //return tab;  //bardzo źle - reguły hermetyzacji

            //poprawnie:
            T[] temp = new T[szczyt + 1];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = tab[i];
            return temp;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<T> ToArrayReadOnly(){
            return Array.AsReadOnly(tab);
        }

        public void TrimExcess(){
            if(IsEmpty)
                return;

            double n = Count * 1.1;
            Array.Resize(ref tab, (int)n);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for(var i = 0; i <= szczyt; i++){
                yield return tab[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        
    }
}
