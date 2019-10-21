using System;
using System.Collections.Generic;
using System.Linq;

/**
 * 
 * Bugged scrambles:
 * 
 * U' R' U R F' U R' F2 R U2
 * 
 *   CubeBot
 *     A program to solve a Rubik's Cube using the Old Pochmann (OP) Blind method.
 *     The OP method is used because it allows us to derive a full solution 
 *     for the cube without making a single turn beforehand. It is also much simpler to 
 *     code, due to its reliance on simple conjugates and then repeated 
 *     algorithms (T or Y Permutation depending on if we are solving edges or corners)
 *     Coded entirely by Tom O'Donnell, project started on 8/3/19
 *     
 *     
 *     
 *     
 *     now make sure cube is solved if shooting to buffer piece
 *     
 *     
 *     
 *     FIX FOR MAJOR TWIST BUG: USE SCRAMBLE R2 U F U F2 R F2 U' F'
 *     
 *     MAKE SURE THAT IF WE ARE SHOOTING TO **ANY STICKER** ON THE PIECE WE SHOT TO IN CASE OF TWISTED BUFFER THEN WE STOP! 
 *     THE ABOVE SCRAMBLE IS A PERFECT EXAMPLE OF WHEN PROGRAM CONTINUES AFTER SOLVING THE CUBE BECAUSE IT DOESN'T KNOW IT SHOT TO NEW BUFFER!
 *     
 *     
 *     
 *     check if shooting to ANY STICKER on new buffer, and if so then buffer is solved (in example scrable if shooting to Q, B, or N
 */


namespace CubeBot
{
    class Program
    {

        //   create array for holding the colors of each corner, plus one extra for storing the next target. [0] is the top-back-right piece. 
        public static string[] CornerColor = new string[10];

        //   if we keep a constant orientation of the cube with white on top and green front, we can assign
        //   static locations where each colored corner piece belongs based on the colors on it.

        public static string targetcolor;
        public static string CornerBelongsInPosition;
        public static string origloc;
        public static string loc;
        public static string setup;
        public static string und;
        public static string YPerm = "R U' R' U' R U R' F' R U R' U' R' F R";

        public static string FinalSolution;
        public static int PossibleNumberOfPiecesToSolve = 7;
        public static List<string> SolvedPieces = new List<string>();
        public static List<string> solutions = new List<string>();
        public static List<string> twisted = new List<string>();
        public static string PieceToSolve;
        public static string[] PiecesToStopAt = { "A", "E", "R", "" };

        public static string twist_buff;
        static bool FinalPiece = false;
        public static int twist = 1;
        public static string colors_on_twist;

        public string RBY = "Back_Bottom_Right";
        public string GYR = "Front_Bottom_Right";
        public string YBO = "Back_Bottom_Left";
        public string OGW = "Front_Top_Left";
        public string GYO = "Front_Bottom_Left";
        public string WBO = "Back_Top_Left";
        public string WRG = "Front_Bottom_Right";
        public string WRB = "Back_Top_Right";


        public static int times_shot = 0;

        public static string posa;
        public static string posb;
        public static string posc;
        public static string posd;
        public static string pose;
        public static string posf;
        public static string posg;
        public static string posh;
        public static string posi;
        public static string posj;
        public static string posk;
        public static string posl;
        public static string posm;
        public static string posn;
        public static string poso;
        public static string posp;
        public static string posq;
        public static string posr;
        public static string poss;
        public static string post;
        public static string posu;
        public static string posv;
        public static string posw;
        public static string posx;
        public static bool handledtwist = false;
        public static bool handlingtwist = false;
        private static bool locistwisted;
        private static bool specialtwistedloccase;
        private static int timesaftersendingtonewbuffer = 0;


        //   This is a pretty simple but tedious process to locate the exact permutation AND orientation in which a piece belongs
        //   Sadly this is also the easiest way as far as I know.
        public static string FindWherePieceBelongs(string CornerColor)
        {
            if (CornerColor.Contains("R") && CornerColor.Contains("B") && CornerColor.Contains("Y"))
            {
                CornerBelongsInPosition = "Back_Bottom_Right";

                if (CornerColor.Substring(0, 1) == "R")
                {
                    loc = "O";
                }
                if (CornerColor.Substring(0, 1) == "B")
                {
                    loc = "T";
                }
                if (CornerColor.Substring(0, 1) == "Y")
                {
                    loc = "W";
                }
            }
            if (CornerColor.Contains("G") && CornerColor.Contains("Y") && CornerColor.Contains("R"))
            {
                CornerBelongsInPosition = "Front_Bottom_Right";

                if (CornerColor.Substring(0, 1) == "G")
                {
                    loc = "K";
                }
                if (CornerColor.Substring(0, 1) == "Y")
                {
                    loc = "V";
                }
                if (CornerColor.Substring(0, 1) == "R")
                {
                    loc = "P";
                }
            }
            if (CornerColor.Contains("Y") && CornerColor.Contains("B") && CornerColor.Contains("O"))
            {
                CornerBelongsInPosition = "Back_Bottom_Left";

                if (CornerColor.Substring(0, 1) == "Y")
                {
                    loc = "X";
                }
                if (CornerColor.Substring(0, 1) == "B")
                {
                    loc = "S";
                }
                if (CornerColor.Substring(0, 1) == "O")
                {
                    loc = "H";
                }
            }
            if (CornerColor.Contains("O") && CornerColor.Contains("G") && CornerColor.Contains("W"))
            {
                CornerBelongsInPosition = "Front_Top_Left";

                if (CornerColor.Substring(0, 1) == "O")
                {
                    loc = "F";
                }
                if (CornerColor.Substring(0, 1) == "G")
                {
                    loc = "I";
                }
                if (CornerColor.Substring(0, 1) == "W")
                {
                    loc = "D";
                }
            }
            if (CornerColor.Contains("G") && CornerColor.Contains("Y") && CornerColor.Contains("O"))
            {
                CornerBelongsInPosition = "Front_Bottom_Left";

                if (CornerColor.Substring(0, 1) == "G")
                {
                    loc = "L";
                }
                if (CornerColor.Substring(0, 1) == "Y")
                {
                    loc = "U";
                }
                if (CornerColor.Substring(0, 1) == "O")
                {
                    loc = "G";
                }
            }
            if (CornerColor.Contains("W") && CornerColor.Contains("B") && CornerColor.Contains("O"))
            {
                CornerBelongsInPosition = "Back_Top_Left";

                if (CornerColor.Substring(0, 1) == "W")
                {
                    loc = "A";
                    PossibleNumberOfPiecesToSolve++;
                    colors_on_twist = "WBO";
                    HandleTwist();
                }
                if (CornerColor.Substring(0, 1) == "B")
                {
                    loc = "R";
                    PossibleNumberOfPiecesToSolve++;
                    colors_on_twist = "BWO";
                    HandleTwist();
                }
                if (CornerColor.Substring(0, 1) == "O")
                {
                    loc = "E";
                    PossibleNumberOfPiecesToSolve++;
                    colors_on_twist = "OBW";
                    HandleTwist();
                }
            }
            if (CornerColor.Contains("W") && CornerColor.Contains("R") && CornerColor.Contains("G"))
            {
                CornerBelongsInPosition = "Front_Top_Right";

                if (CornerColor.Substring(0, 1) == "W")
                {
                    loc = "C";
                }
                if (CornerColor.Substring(0, 1) == "R")
                {
                    loc = "M";
                }
                if (CornerColor.Substring(0, 1) == "G")
                {
                    loc = "J";
                }
            }
            if (CornerColor.Contains("W") && CornerColor.Contains("R") && CornerColor.Contains("B"))
            {
                CornerBelongsInPosition = "Back_Top_Right";

                if (CornerColor.Substring(0, 1) == "W")
                {
                    loc = "B";
                }
                if (CornerColor.Substring(0, 1) == "R")
                {
                    loc = "N";
                }
                if (CornerColor.Substring(0, 1) == "B")
                {
                    loc = "Q";
                }
            }


            //Console.WriteLine(CornerBelongsInPosition);
            //Console.WriteLine("");
            //if (loc == twist_buff)
            //{                                                                                             //tried cutting this for a second
            //targetcolor = colors_on_twist;
            //Console.WriteLine("shooting to new buffer");
            //}
            Console.WriteLine("Corner " + CornerColor + " belongs in position " + loc);
            //if (ShootingToBuffer(loc))
            //{
            //Console.WriteLine("we are shooting to " + loc + " which is on our new buffer, meaning the new and old buffer will be solved.");
            //FinalPiece = true;
            //}
            return CornerBelongsInPosition;
        }

        public static bool ShootingToBuffer(string loc)
        {
            if (twist_buff == "B") //if our new buffer that we shot to is on the piece where sticker B resides
            {
                if (loc == "B" || loc == "N" || loc == "Q") //and if we are shooting to any piece on that buffer
                {
                    return true; //then yes, we are shooting to the buffer and the buffer is now solved (meaning either the cube is solved or we have a solved buffer but pieceslefttosolve > 0 so we still have some pieces left to solve (nned new cycle)
                }
            }
            if (twist_buff == "C")
            {
                if (loc == "C" || loc == "M" || loc == "J")
                {
                    return true;
                }
            }
            if (twist_buff == "D")
            {
                if (loc == "D" || loc == "F" || loc == "I")
                {
                    return true;
                }
            }
            if (twist_buff == "L")
            {
                if (loc == "L" || loc == "L" || loc == "U")
                {
                    return true;
                }
            }
            if (twist_buff == "K")
            {
                if (loc == "K" || loc == "P" || loc == "V")
                {
                    return true;
                }
            }
            if (twist_buff == "W")
            {
                if (loc == "W" || loc == "O" || loc == "T")
                {
                    return true;
                }
            }
            if (twist_buff == "X")
            {
                if (loc == "X" || loc == "H" || loc == "S")
                {
                    return true;
                }
            }
            return false; //if none of the above, return false
        }


        public static string FindSetupMoves(string loc)
        {
            if (loc == "A")
            {
                setup = "";
                und = "";
            }
            if (loc == "B")
            {
                setup = "R D'";
                und = "D R'";
            }
            if (loc == "C")
            {
                setup = "F";
                und = "F'";
            }
            if (loc == "D")
            {
                setup = "L D L'";
                und = "L D' L'";
            }
            if (loc == "E")
            {
                setup = "";
                und = "";
                Console.WriteLine("ERR: Buffer belongs in a buffer position (twisted buffer or need new cycle)");
                Environment.Exit(1);
            }
            if (loc == "F")
            {
                setup = "F2";
                und = "F2";
            }
            if (loc == "G")
            {
                setup = "D2 R";
                und = "R' D2";
            }
            if (loc == "H")
            {
                setup = "D2";
                und = "D2";
            }
            if (loc == "I")
            {
                setup = "F' D";
                und = "D' F";
            }
            if (loc == "J")
            {
                setup = "F2 D";
                und = "D' F2";
            }
            if (loc == "K")
            {
                setup = "F D";
                und = "D' F'";
            }
            if (loc == "L")
            {
                setup = "D";
                und = "D'";
            }
            if (loc == "M")
            {
                setup = "R'";
                und = "R";
            }
            if (loc == "N")
            {
                setup = "R2";
                und = "R2";
            }
            if (loc == "O")
            {
                setup = "R";
                und = "R'";
            }
            if (loc == "P")
            {
                setup = "";
                und = "";
            }
            if (loc == "Q")
            {
                setup = "R' F";
                und = "F' R";
            }
            if (loc == "R")
            {
                setup = "";
                und = "";
                Console.WriteLine("ERR: Buffer belongs in a buffer position (twisted buffer or need new cycle)");
                Environment.Exit(1);
            }
            if (loc == "S")
            {
                setup = "D' R";
                und = "R' D";
            }
            if (loc == "T")
            {
                setup = "D'";
                und = "D";
            }
            if (loc == "U")
            {
                setup = "F'";
                und = "F";
            }
            if (loc == "V")
            {
                setup = "D' F'";
                und = "F D";
            }
            if (loc == "W")
            {
                setup = "D2 F'";
                und = "F D2";
            }
            if (loc == "X")
            {
                setup = "D F'";
                und = "F D'";
            }

            return setup;
        }


        public static void FindNextPiece(string loc)
        {
            if (loc == "A" || loc == "E" || loc == "R")
            {
                if (posa != "W")
                {
                    //either solved or we need to start a new cycle (or twisted buffer)
                }
            }
            //if (solved)
            //{
            //   Console.WriteLine("cube is solved");
            //  Environment.Exit(0);
            //}

            Console.WriteLine("loc is " + loc);

            //Console.WriteLine(posf + posd + posi);


            //if (loc == twist_buff) //if we are shooting to where we shot in case of a twisted buffer
            //{
            //  times_shot++;
            //if (times_shot == 1) //TRY CHANGING TO 1 seems doesn't trigger when actaully shooting to it fi this


            //do if shooting to new buffer and pieces remaining != 0 then buffer is solved but other pieces aren't so shoot to new position like a cornertwist
            //{
            //Console.WriteLine("We are shooting to where we shot the buffer because of a twisted corner");
            //  loc = twist_buff;
            // targetcolor = colors_on_twist;
            //times_shot = 0;
            //}
            //}


            if (loc == "B")
            {
                targetcolor = posb + posn + posq;
            }
            if (loc == "C")
            {
                targetcolor = posc + posm + posj;
            }
            if (loc == "D")
            {
                targetcolor = posd + posf + posi;
            }
            if (loc == "F")
            {
                targetcolor = posf + posd + posi;
                //Console.WriteLine(f + d + i);
            }
            if (loc == "G")
            {
                targetcolor = posg + posu + posl;
            }
            if (loc == "H")
            {
                targetcolor = posh + posx + poss;
            }
            if (loc == "I")
            {
                targetcolor = posi + posd + posf;
            }
            if (loc == "J")
            {
                targetcolor = posj + posc + posm;
            }
            if (loc == "K")
            {
                targetcolor = posk + posp + posv;
            }
            if (loc == "L")
            {
                targetcolor = posl + posg + posu;
            }
            if (loc == "M")
            {
                targetcolor = posm + posc + posj;
            }
            if (loc == "N")
            {
                targetcolor = posn + posb + posq;
            }
            if (loc == "O")
            {
                targetcolor = poso + post + posw;
            }
            if (loc == "P")
            {
                targetcolor = posp + posk + posv;
            }
            if (loc == "Q")
            {
                targetcolor = posq + posb + posn;
            }
            if (loc == "S")
            {
                targetcolor = poss + posh + posx;
            }
            if (loc == "T")
            {
                targetcolor = post + poso + posw;
            }
            if (loc == "U")
            {
                targetcolor = posu + posl + posg;
            }
            if (loc == "V")
            {
                targetcolor = posv + posk + posp;
            }
            if (loc == "W")
            {
                targetcolor = posw + poso + post;
            }
            if (loc == "X")
            {
                targetcolor = posx + posh + poss;
            }

            Console.WriteLine("next piece to solve is " + targetcolor);
            Console.WriteLine("loc is currently" + loc);
        }


        public static void Main(string[] args)
        {

            Console.WriteLine("CubeBot: Solves Rubik's Cubes using Tom O'Donnell's self-designed recognition and solving algorithms");
            Console.WriteLine("Press enter to begin.");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            Console.WriteLine("Enter corner 1 color");
            CornerColor[0] = Console.ReadLine();
            posa = CornerColor[0].Substring(0, 1);
            pose = CornerColor[0].Substring(1, 1);
            posr = CornerColor[0].Substring(2, 1);

            Console.WriteLine("Enter corner 2 color");
            CornerColor[1] = Console.ReadLine();
            posb = CornerColor[1].Substring(0, 1);
            posn = CornerColor[1].Substring(1, 1);
            posq = CornerColor[1].Substring(2, 1);

            Console.WriteLine("Enter corner 3 color");
            CornerColor[2] = Console.ReadLine();
            posc = CornerColor[2].Substring(0, 1);
            posm = CornerColor[2].Substring(1, 1);
            posj = CornerColor[2].Substring(2, 1);


            Console.WriteLine("Enter corner 4 color");
            CornerColor[3] = Console.ReadLine();
            posd = CornerColor[3].Substring(0, 1);
            posf = CornerColor[3].Substring(1, 1);
            posi = CornerColor[3].Substring(2, 1);



            Console.WriteLine("Enter corner 5 color");
            CornerColor[4] = Console.ReadLine();
            posu = CornerColor[4].Substring(0, 1);
            posg = CornerColor[4].Substring(1, 1);
            posl = CornerColor[4].Substring(2, 1);


            Console.WriteLine("Enter corner 6 color");
            CornerColor[5] = Console.ReadLine();
            posv = CornerColor[5].Substring(0, 1);
            posp = CornerColor[5].Substring(1, 1);
            posk = CornerColor[5].Substring(2, 1);


            Console.WriteLine("Enter corner 7 color");
            CornerColor[6] = Console.ReadLine();
            posw = CornerColor[6].Substring(0, 1);
            poso = CornerColor[6].Substring(1, 1);
            post = CornerColor[6].Substring(2, 1);


            Console.WriteLine("Enter corner 8 color");
            CornerColor[7] = Console.ReadLine();
            posx = CornerColor[7].Substring(0, 1);
            posh = CornerColor[7].Substring(1, 1);
            poss = CornerColor[7].Substring(2, 1);



            //foreach (string str in CornerColor)
            //{
            //  Console.WriteLine(str);
            //}

            FindIfAnySolvedPieces();

            if (PossibleNumberOfPiecesToSolve == 0)
            {
                Console.WriteLine("Your cube is already solved, you dummy.");
                Environment.Exit(0);
            }



            FindWherePieceBelongs(CornerColor[0]); //find where buffer belongs
            FindSetupMoves(loc); //find which setup moves/inverse we need to use
            AddToSolvedPieces(loc);
            Console.WriteLine("Corner Solution: " + setup + " " + YPerm + " " + und); //print solution for first corner
            PossibleNumberOfPiecesToSolve--;
            solutions.Add(setup + " " + YPerm + " " + und);
            SolvedPieces.ForEach(i => Console.Write("{0}\t", i));

            if (PossibleNumberOfPiecesToSolve > 0)
            {
                FindNextPiece(loc); //find the colors of the piece in the position we just shot to, and store it in `targetcolor`
                CornerColor[8] = targetcolor; //Store `targetcolor` as CornerColor[8]
                FindWherePieceBelongs(CornerColor[8]); //find where CornerColor[8] belongs, or where the piece we just shot to belongs
                FindSetupMoves(loc); //find setup moves for that piece
                Console.WriteLine("Corner Solution: " + setup + " " + YPerm + " " + und); //print solution for that piece
                solutions.Add(setup + " " + YPerm + " " + und);
                AddToSolvedPieces(loc);
                SolvedPieces.ForEach(i => Console.Write("{0}\t", i));
                PossibleNumberOfPiecesToSolve--; //since we solved a corner, subtract one from how many corners we have to solve
            }

            while (PossibleNumberOfPiecesToSolve > 0)
            {
                SolveNextCorner();
            } //keep solving corners until we have none left to solve :)


            if (PossibleNumberOfPiecesToSolve == 0)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Your cube is solved");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Full solution to your cube: ");
                Console.WriteLine(Environment.NewLine);
                solutions.ForEach(i => Console.Write("{0} ", i));
                Environment.Exit(0);
            }

        }

        public static void AddToSolvedPieces(string loc)
        {
            if (loc == "B" || loc == "N" || loc == "Q")
            {
                SolvedPieces.Add("TBR");
            }
            if (loc == "C" || loc == "M" || loc == "J")
            {
                SolvedPieces.Add("TFR");
            }
            if (loc == "D" || loc == "F" || loc == "I")
            {
                SolvedPieces.Add("TFL");
            }
            if (loc == "U" || loc == "G" || loc == "L")
            {
                SolvedPieces.Add("BFL");
            }
            if (loc == "V" || loc == "P" || loc == "K")
            {
                SolvedPieces.Add("BFR");
            }
            if (loc == "W" || loc == "O" || loc == "T")
            {
                SolvedPieces.Add("BBR");
            }
            if (loc == "X" || loc == "H" || loc == "S")
            {
                SolvedPieces.Add("BBL");
            }
        }

        public static void HandleTwist()
        {
            //we have a twisted buffer. In order to solve it, we must find a piece we haven't shot to yet, and shoot to it.
            //Then we continue like normal.
            //twist++;
            handlingtwist = true;
            Console.WriteLine("twisted corner...");
            //Environment.Exit(1);
            FindPieceToSolve();
        }

        public static void CheckIfLocIsTwisted(string loc)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("checking if loc is twisted... loc is now " + loc);
            Console.WriteLine(Environment.NewLine);
            if (loc == "B")
            {
                if (twisted.Contains("TBR"))
                {
                    locistwisted = true;
                }
            }
            if (loc == "C")
            {
                if (twisted.Contains("TFR"))
                {
                    locistwisted = true;
                }
            }
            if (loc == "D")
            {
                if (twisted.Contains("TFL"))
                {
                    locistwisted = true;
                }
            }
            if (loc == "L")
            {
                if (twisted.Contains("BFL"))
                {
                    locistwisted = true;
                }
            }
            if (loc == "K")
            {
                if (twisted.Contains("BFR"))
                {
                    locistwisted = true;
                }
            }
            if (loc == "W")
            {
                if (twisted.Contains("BBR"))
                {
                    locistwisted = true;
                }
            }
            if (loc == "X")
            {
                if (twisted.Contains("BBL"))
                {
                    locistwisted = true;
                }
            }
        }

        public static void FindPieceToSolve()
        {
            CornerColor[9] = colors_on_twist; //set colors_on_twist as cornercolor[9]
            Console.WriteLine("Corner orientation is " + colors_on_twist);
            //if (FindWherePieceBelongs(CornerColor[9]) == colors_on_twist)
            //{
            //Console.WriteLine("bad idea");
            //}


            if (!SolvedPieces.Contains("TBR"))
            {
                loc = "B";
                twist_buff = "B"; //we use "loc" here because we need to know if we're shooting to ANY sticker on buffer piece
                Console.WriteLine(loc + " is not solved. Shooting to " + loc);
                CheckIfLocIsTwisted(loc);
                if (locistwisted)
                {
                    Console.WriteLine("We are trying to shoot to a piece that is twisted in place....");
                    specialtwistedloccase = true;
                }
                return;
            }
            if (!SolvedPieces.Contains("TFR"))
            {
                loc = "C";
                Console.WriteLine(loc + " is not solved. Shooting to " + loc);
                twist_buff = loc;
                CheckIfLocIsTwisted(loc);
                if (locistwisted)
                {
                    Console.WriteLine("We are trying to shoot to a piece that is twisted in place....");
                    specialtwistedloccase = true;
                }
                return;
            }
            if (!SolvedPieces.Contains("TFL"))
            {
                loc = "D";
                Console.WriteLine(loc + " is not solved. Shooting to " + loc);
                twist_buff = loc;
                CheckIfLocIsTwisted(loc);
                if (locistwisted)
                {
                    Console.WriteLine("We are trying to shoot to a piece that is twisted in place....");
                    specialtwistedloccase = true;
                }
                return;
            }
            if (!SolvedPieces.Contains("BFL"))
            {
                loc = "L";
                Console.WriteLine(loc + " is not solved. Shooting to " + loc);
                twist_buff = loc;
                CheckIfLocIsTwisted(loc);
                if (locistwisted)
                {
                    Console.WriteLine("We are trying to shoot to a piece that is twisted in place....");
                    specialtwistedloccase = true;
                }
                return;
            }
            if (!SolvedPieces.Contains("BFR"))
            {
                loc = "K";
                Console.WriteLine(loc + " is not solved. Shooting to " + loc);
                twist_buff = loc;
                CheckIfLocIsTwisted(loc);
                if (locistwisted)
                {
                    Console.WriteLine("We are trying to shoot to a piece that is twisted in place....");
                    specialtwistedloccase = true;
                }
                return;
            }
            if (!SolvedPieces.Contains("BBR"))
            {
                loc = "W";
                Console.WriteLine(loc + " is not solved. Shooting to " + loc);
                twist_buff = loc;
                CheckIfLocIsTwisted(loc);
                if (locistwisted)
                {
                    Console.WriteLine("We are trying to shoot to a piece that is twisted in place....");
                    specialtwistedloccase = true;
                }
                return;
            }
            if (!SolvedPieces.Contains("BBL"))
            {
                loc = "X";
                Console.WriteLine(loc + " is not solved. Shooting to " + loc);
                twist_buff = loc;
                CheckIfLocIsTwisted(loc);
                if (locistwisted)
                {
                    Console.WriteLine("We are trying to shoot to a piece that is twisted in place....");
                    specialtwistedloccase = true;
                }
                return;
            }
        }

        public static void SolveNextCorner()
        {
            FindNextPiece(loc); //find the colors of the piece in the position we just shot to, and store it in `targetcolor`
            CornerColor[8] = targetcolor; //Store `targetcolor` as CornerColor[8]
            FindWherePieceBelongs(CornerColor[8]); //find where CornerColor[8] belongs, or where the piece we just shot to belongs


            if (ShootingToBuffer(loc))
            {
                Console.WriteLine("we are shooting to buffer");
                if (!handlingtwist)
                {
                    twist++;
                    Console.WriteLine("adding 1 to twist");
                    if (twist == 2)
                    {
                        Console.WriteLine("we are shooting to buffer so cube should be solved or need new cycle after this");
                    }
                }
            }

            if (twist_buff != null)
            {
                if (handledtwist)
                {
                    //Console.WriteLine("restoring original loc from " + loc + " to " + twist_buff);
                    //origloc = loc;
                    //loc = twist_buff;
                    handledtwist = false;
                }
            }

            Console.WriteLine("loc is " + loc);
            FindSetupMoves(loc); //find setup moves for that piece
            PossibleNumberOfPiecesToSolve--;
            Console.WriteLine(PossibleNumberOfPiecesToSolve);
            Console.WriteLine("Corner Solution: " + setup + " " + YPerm + " " + und); //print solution for that piece
            AddToSolvedPieces(loc);
            solutions.Add(setup + " " + YPerm + " " + und);
            Console.WriteLine("We have solved: " + SolvedPieces);
            SolvedPieces.ForEach(i => Console.Write("{0}\t", i));


            if (specialtwistedloccase)
            {

            }


            if (PossibleNumberOfPiecesToSolve == 0)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Your cube is solved");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Full solution to your cube: ");
                Console.WriteLine(Environment.NewLine);
                solutions.ForEach(i => Console.Write("{0} ", i));
                Environment.Exit(0);
            }

            if (ShootingToBuffer(loc))
            {
                if (twist == 2)
                {
                    SolvedPieces.ForEach(i => Console.Write("{0}\t", i));
                    Environment.Exit(0);
                }
            }

            if (FinalPiece)
            {
                Environment.Exit(0);
            }

            if (PossibleNumberOfPiecesToSolve == 0)
            {
                if (PiecesToStopAt.Contains(loc)) //if our loc is a buffer position or the piece we shot to in a new cycle (in case of twisted buffer)
                {
                    Console.WriteLine("Cube is solved!");
                    Console.WriteLine("Full solution to your cube: ");
                    Console.WriteLine(Environment.NewLine);
                    solutions.ForEach(i => Console.Write("{0} ", i));
                    Environment.Exit(0);
                    Environment.Exit(0);
                }
            }

            if (handlingtwist)
            {
                handledtwist = true;
                handlingtwist = false;
            }

            if (specialtwistedloccase)
            {
                Console.WriteLine("adding one to times after shooting to new buffer");
                timesaftersendingtonewbuffer++;
            }
        }

        public static void FindIfAnySolvedPieces()
        {
            if (CornerColor[0].Contains("W") && CornerColor[0].Contains("O") && CornerColor[0].Contains("B"))
            {
                if (CornerColor[0].Substring(0, 1) == "W")
                {
                    PossibleNumberOfPiecesToSolve--;
                    SolvedPieces.Add("TBL");
                }
            }
            if (CornerColor[1].Contains("W") && CornerColor[1].Contains("R") && CornerColor[1].Contains("B"))
            {
                if (CornerColor[1].Substring(0, 1) == "W")
                {
                    PossibleNumberOfPiecesToSolve--;
                    SolvedPieces.Add("TBR");
                }
            }
            if (CornerColor[2].Contains("W") && CornerColor[2].Contains("R") && CornerColor[2].Contains("G"))
            {
                if (CornerColor[2].Substring(0, 1) == "W")
                {
                    PossibleNumberOfPiecesToSolve--;
                    SolvedPieces.Add("TFR");
                }
            }
            if (CornerColor[3].Contains("W") && CornerColor[3].Contains("O") && CornerColor[3].Contains("G"))
            {
                if (CornerColor[3].Substring(0, 1) == "W")
                {
                    PossibleNumberOfPiecesToSolve--;
                    SolvedPieces.Add("TFL");
                }
            }
            if (CornerColor[4].Contains("Y") && CornerColor[4].Contains("O") && CornerColor[4].Contains("G"))
            {
                //do the same for Y
                if (CornerColor[4].Substring(0, 1) == "Y")
                {
                    PossibleNumberOfPiecesToSolve--;
                    SolvedPieces.Add("BFL");
                }
            }
            if (CornerColor[5].Contains("Y") && CornerColor[5].Contains("R") && CornerColor[5].Contains("G"))
            {
                if (CornerColor[5].Substring(0, 1) == "Y")
                {
                    PossibleNumberOfPiecesToSolve--;
                    SolvedPieces.Add("BFR");
                }
            }
            if (CornerColor[6].Contains("Y") && CornerColor[6].Contains("R") && CornerColor[6].Contains("B"))
            {
                if (CornerColor[6].Substring(0, 1) == "Y")
                {
                    PossibleNumberOfPiecesToSolve--;
                    SolvedPieces.Add("BBR");
                }
            }
            if (CornerColor[7].Contains("Y") && CornerColor[7].Contains("O") && CornerColor[7].Contains("B"))
            {
                if (CornerColor[7].Substring(0, 1) == "Y")
                {
                    PossibleNumberOfPiecesToSolve--;
                    SolvedPieces.Add("BBL");
                }
            }

            Console.WriteLine("Number of pieces we need to solve: " + PossibleNumberOfPiecesToSolve);

            FindIfAnyTwistedPieces();
            Console.WriteLine("Pieces that are permuted correctly but not oriented correctly (aka twisted): ");
            twisted.ForEach(i => Console.Write("{0} ", i));

        }

        public static void FindIfAnyTwistedPieces()
        {
            if (CornerColor[0].Contains("W") && CornerColor[0].Contains("O") && CornerColor[0].Contains("B"))
            {
                if (CornerColor[0].Substring(0, 1) != "W")
                {
                    twisted.Add("TBL");
                }
            }
            if (CornerColor[1].Contains("W") && CornerColor[1].Contains("R") && CornerColor[1].Contains("B"))
            {
                if (CornerColor[1].Substring(0, 1) != "W")
                {
                    twisted.Add("TBR");
                }
            }
            if (CornerColor[2].Contains("W") && CornerColor[2].Contains("R") && CornerColor[2].Contains("G"))
            {
                if (CornerColor[2].Substring(0, 1) != "W")
                {
                    twisted.Add("TFR");
                }
            }
            if (CornerColor[3].Contains("W") && CornerColor[3].Contains("O") && CornerColor[3].Contains("G"))
            {
                if (CornerColor[3].Substring(0, 1) != "W")
                {
                    twisted.Add("TFL");
                }
            }
            if (CornerColor[4].Contains("Y") && CornerColor[4].Contains("O") && CornerColor[4].Contains("G"))
            {
                if (CornerColor[4].Substring(0, 1) != "Y")
                {
                    twisted.Add("BFL");
                }
            }
            if (CornerColor[5].Contains("Y") && CornerColor[5].Contains("R") && CornerColor[5].Contains("G"))
            {
                if (CornerColor[5].Substring(0, 1) != "Y")
                {
                    twisted.Add("BFR");
                }
            }
            if (CornerColor[6].Contains("Y") && CornerColor[6].Contains("R") && CornerColor[6].Contains("B"))
            {
                if (CornerColor[6].Substring(0, 1) != "Y")
                {
                    twisted.Add("BBR");
                }
            }
            if (CornerColor[7].Contains("Y") && CornerColor[7].Contains("O") && CornerColor[7].Contains("B"))
            {
                if (CornerColor[7].Substring(0, 1) != "Y")
                {
                    twisted.Add("BBL");
                }
            }
        }
    }
}
