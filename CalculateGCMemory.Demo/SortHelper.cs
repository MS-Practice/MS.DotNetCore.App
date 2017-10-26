using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateGCMemory.Demo
{
    public class SortHelper
    {
        public void QuickSort(int[] array)
        {
            QuickSortInternal(array, 0, array.Length - 1);
        }
        private void QuickSortInternal(int[] array,int begin,int end)
        {
            if(begin == end)
            {
                return;
            }
            else
            {
                int pivot = FindPivotIndex(array, begin, end);
                if (pivot > begin) QuickSortInternal(array, begin, pivot - 1);
                if (pivot < end) QuickSortInternal(array, pivot + 1, end);
            }
        }
        private int FindPivotIndex(int[] array, int begin, int end)
        {
            int pivot = begin;
            int m = begin + 1;
            int n = end;

            while (m < end && array[pivot] >= array[m])
            {
                m++;
            }

            while (n > begin && array[pivot] <= array[n])
            {
                n--;
            }

            while (m < n)
            {
                int temp = array[m];
                array[m] = array[n];
                array[n] = temp;

                while (m < end && array[pivot] >= array[m])
                {
                    m++;
                }

                while (n > begin && array[pivot] <= array[n])
                {
                    n--;
                }
            }

            if (pivot != n)
            {
                int temp = array[n];
                array[n] = array[pivot];
                array[pivot] = temp;

            }

            return n;
        }
    }
}
