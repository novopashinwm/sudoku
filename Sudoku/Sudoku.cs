using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sudoku
{
    delegate void PrintDigitDelegate(int x, int y, int d);
    delegate void SaveAnswerDelegate();

    class Sudoku
    {
        public const int max = 9; // размер поля
        public const int sqr = 3; // размер квадратика

        public int[,] map { get; private set; }

        PrintDigitDelegate  PrintDigit;
        SaveAnswerDelegate SaveAnswer;

        public Sudoku(PrintDigitDelegate printDigit
            , SaveAnswerDelegate saveAnswer) {
            map = new int[max, max];
            PrintDigit = printDigit;
            SaveAnswer = saveAnswer;
            ClearMap();

        }

        public void ClearMap() {
            for (int x = 0; x < max; x++)
                for (int y = 0; y < max; y++)
                    ClearDigit(x, y);
        }

        public bool PlaceDigit(int x, int y, int d){
            if (x < 0 || x >= max) return false;
            if (y < 0 || y >= max) return false;
            if (d <= 0 || d > Sudoku.max) return false;

            if (map[x, y] == d) return true;
            if (map[x, y] != 0) return false;

            for (int cx = 0; cx < max; cx++)
                if (map[cx, y] == d) return false;

            for (int cy = 0; cy < max; cy++)
                if (map[x, cy] == d) return false;
            //Координата левого верхнего угла текущего квадрата
            int sx = Sudoku.sqr * (x / Sudoku.sqr);
            int sy = Sudoku.sqr * (y / Sudoku.sqr);

            for (int cx = sx; cx < sx + sqr; cx++)
            for (int cy = sy; cy < sy + sqr; cy++)
                if (map[cx, cy] == d) return false;

            map[x, y] = d;
            PrintDigit(x, y, d);
            return true;
        }

        //Найдено ли решение
        bool found = false;
        public int count = 0;
        public bool Solve()
        {
            count = 0;
            NextDigit(0); //вызываем с начальной клетки
            return found;
        }

        void NextDigit(int step) {
            if (found) return;

            if (step == max * max){
                found = true;
                SaveAnswer();
                SaveAnswerPrivate();
                return;
            }
            count++;
            int x = step % max;
            int y = step / max;

            if (map[x, y] > 0) {
                NextDigit(step + 1);
                return;
            }

            for (int d = 1; d <=max; d++)
                if (PlaceDigit(x, y, d))
                {
                    NextDigit(step + 1);
                    ClearDigit(x, y);
                }
        }

        void ClearDigit(int x, int y) {
            if (x < 0 || x >= max) return;
            if (y < 0 || y >= max) return;
            if (map[x, y] == 0) return;
            map[x, y] = 0;
            PrintDigit(x, y, 0);
            
        }

        void SaveAnswerPrivate(){
            Console.SetCursorPosition(0, max + sqr + 1);
            Console.Write("Количество вызовов рекурсивной функции - {0}", count);
            Console.ReadKey();
        }


    }
}
