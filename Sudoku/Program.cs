using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Sudoku
{
    class Program
    {
        Sudoku sudoku;
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start();
            Console.ReadKey();
        }

        public void Start() {
            PrintFrame();
            sudoku = new Sudoku(PrintDigit, SaveAnswer);
            LoadSudoku("sudoku.txt");
            sudoku.Solve();
            /*do {
                Console.ForegroundColor = ConsoleColor.Gray;
                GenerateRandom(70);            
                Console.ForegroundColor = ConsoleColor.Green;
                Console.ReadKey();
            } while (!sudoku.Solve());
            */
            
        }

        void SaveAnswer(){
            using (StreamWriter file = new StreamWriter("solver.txt", false, Encoding.GetEncoding(1251) )){
                string symbol = " ";
                for (int py = 0; py <= Sudoku.max + Sudoku.sqr; py++)
                {
                    for (int px = 0; px <= Sudoku.max + Sudoku.sqr; px++)
                    {
                        if (px % (Sudoku.sqr + 1) == 0 &&
                            py % (Sudoku.sqr + 1) == 0)
                            symbol = "+";
                        else
                            if (px % (Sudoku.sqr + 1) == 0)
                                symbol = "|";
                            else
                                if (py % (Sudoku.sqr + 1) == 0)
                                    symbol = "-";
                                else
                                {
                                    int x = px - 1 - px / (Sudoku.sqr + 1);
                                    int y = py - 1 - py / (Sudoku.sqr + 1);
                                    symbol = sudoku.map[x, y].ToString();
                                }
                        file.Write(symbol);
                    }
                    file.WriteLine();
                }
                file.WriteLine("Головоломка решена за {0} шагов.", sudoku.count);
            }
        }

        private void GenerateRandom(int count)
        {
            Random objRandom = new Random();
            if (count == 0 ||
                count> Sudoku.max * Sudoku.max) return;
            sudoku.ClearMap();
            for (int c = 0; c < count; c++)
            {
                int x=0, y=0, d=0, loop=500;
                do {
                    x = objRandom.Next(0, Sudoku.max);
                    y = objRandom.Next(0, Sudoku.max);
                    d = objRandom.Next(1, Sudoku.max+1);
                } while (--loop>0 && !sudoku.PlaceDigit (x,y, d));
            }
            for (int x = 0; x < Sudoku.max; x++)
            for (int y = 0; y < Sudoku.max; y++)
                sudoku.PlaceDigit(x, y, objRandom.Next(1, Sudoku.max+1));
        }

        public void LoadSudoku(string filename) {
            string[] lines = File.ReadAllLines(filename);
            int y=0; 
            for (int i = 0; i < lines.Length; i++)
            {
                string s = lines[i];
                if (lines[i].Contains("---")) continue;
                s = s.Replace("!", "");
                s = s.Replace('.', '0');
                for (int x = 0; x < s.Length; x++)                
                    sudoku.PlaceDigit (x,y, int.Parse ( s.Substring (x,1)));                
                y++;
            }
        }
        
        public void PrintFrame() {

            string symbol =" ";
            for (int px = 0; px <= (Sudoku.sqr+1) * Sudoku.sqr; px++)            
            for (int py = 0; py <= Sudoku.max +  Sudoku.sqr; py++)
            {
                if (px % (Sudoku.sqr + 1) == 0 &&
                    py % (Sudoku.sqr + 1) == 0)
                    symbol = "+";
                else
                    if (px % (Sudoku.sqr + 1) == 0)
                        symbol = "|";
                    else
                        if (py % (Sudoku.sqr + 1) == 0)
                            symbol = "-";
                        else
                            symbol = " ";
                Console.SetCursorPosition(px, py);
                Console.Write(symbol);                
            }
            
        }

        public void PrintDigit(int x, int y, int d){
            int px = 1 + x + x / Sudoku.sqr;
            int py = 1 + y + y / Sudoku.sqr;
            Console.SetCursorPosition(px, py);
            Console.Write(d==0? " ": d.ToString());
            Thread.Sleep(1);
        }

        
    }
}
