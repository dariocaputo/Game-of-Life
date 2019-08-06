using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JUEGO_DE_LA_VIDA
{
    class Program
    {
        const int FIL = 20;
        const int COL = 60;

        static void CargarMatrizManual(int[,] matriz)
        {
            int cantCelulasVivas = 0, celulasColocadas = 0, fila, columna;

            Console.WriteLine("Dispone de un espacio celular de " + FIL + " filas y " + COL + " columnas");
            Console.WriteLine("En total puede colocar " + FIL * COL + " células");
            Console.WriteLine("\n¿Cuántas células vivas desea colocar?\n");
            cantCelulasVivas = LeerNum();
            EvaluarLimites(ref cantCelulasVivas, 1, FIL * COL);
            Console.Clear();

            while (celulasColocadas < cantCelulasVivas)
            {
                do
                { 
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\t\t\t\t\t***ESPACIO CELULAR***\n");
                    Console.ResetColor();
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------\n");
                    MostrarConfigInicial(matriz);
                    Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------------\n");
                    Console.WriteLine("\nCélulas colocadas: " + celulasColocadas);
                    Console.WriteLine("Le quedan: " + (cantCelulasVivas - celulasColocadas) + " células");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nELIJA LA POSICIÓN DE SU CÉLULA...\n");
                    Console.ResetColor();                
                
                    fila = IngresarNumero("Fila: ", 1, FIL) - 1;
                    columna = IngresarNumero("Columna: ", 1, COL) - 1;
                    if (matriz[fila, columna] != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n¡POSICIÓN OCUPADA!. Ingrese otra...");                        
                        System.Threading.Thread.Sleep(2000);
                        Console.Clear();
                    }                   

                } while (matriz[fila, columna] != 0);           

                matriz[fila, columna] = 1;
                celulasColocadas++;
                Console.Clear();
            }
        }

        static void CargarMatrizAleatoria(int[,] matriz)
        {
            Random rdm = new Random();
            int cantCelulasVivas = 0, celulasColocadas = 0, fila, columna;

            Console.WriteLine("Dispone de un espacio celular de " + FIL + " filas y " + COL + " columnas");
            Console.WriteLine("En total puede colocar " + FIL * COL + " células");
            Console.WriteLine("\n¿Cuántas células vivas desea colocar?\n");
            cantCelulasVivas = LeerNum();
            EvaluarLimites(ref cantCelulasVivas, 1, FIL * COL);
            Console.Clear();

            while (celulasColocadas < cantCelulasVivas)
            {
                do
                {
                    fila = rdm.Next(FIL);
                    columna = rdm.Next(COL);

                } while (matriz[fila, columna] != 0);

                matriz[fila, columna] = 1;
                celulasColocadas++;
            }
        }

        static int IngresarNumero(string pedido, int min, int max)
        {
            int num;
            Console.WriteLine(pedido);
            num = LeerNum();
            while (num < min || num > max)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nFuera de rango, reingrese: ");
                Console.ResetColor();               
                num = LeerNum();
            }
            return num;
        }

        static void MostrarConfigInicial(int[,] matriz)
        {
            Console.CursorVisible = false;
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    if (matriz[i, j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write("■");
                    Console.Write(matriz[i, j]);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static void ImprimirMatriz(int[,] matriz, int turno, int muertes, int nacimientos, ref int celVivas, ref int celMuertas)
        {
            Console.CursorVisible = false;//Oculta el cursor
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t\t\t\t***SIMULADOR DE AUTÓMATA CELULAR***\n");
            Console.ResetColor();
            StringBuilder constructor = new StringBuilder();
            celVivas = 0;
            celMuertas = 0; 
            char viva = '\u2588';//Unicode para FULL BLOCK
            char muerta = ' ';
            char celula;
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    if (matriz[i, j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        celula = viva;
                        celVivas++;
                    }
                    else
                    {
                        celula = muerta;
                        celMuertas++;
                    }
                    constructor.Append(celula);
                    constructor.Append(celula);
                }
                constructor.Append("\n");
            }
            Console.WriteLine(constructor.ToString());
            Console.ResetColor();

            Console.WriteLine("\n----------------------------------------\n");
            Console.WriteLine("TURNO: " + turno + "\n");
            Console.WriteLine("Células totales: " + (celVivas + celMuertas));
            Console.WriteLine("Células vivas: " + celVivas);
            Console.WriteLine("Células muertas: " + celMuertas);
            Console.WriteLine("Nacimientos totales: " + nacimientos + "\nMuertes totales: " + muertes + "\n");
            Console.WriteLine("----------------------------------------\n");
        }

        static void EvaluarLimites(ref int num, int limiteInferior, int limiteSuperior)
        {
            while (num < limiteInferior || num > limiteSuperior)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("El número debe estar entre " + limiteInferior + " y " + limiteSuperior + ". Reingrese...");
                Console.ResetColor();
                num = LeerNum();
            }
        }

        static void MenuTurnos(int[,] matriz, int turno, int nacimientos, int muertes)
        {
            int[,] matrizFutura = new int[FIL, COL];
            int nacimientosCongelados = 0, muertesCongeladas = 0, turnosCongelados = 0, celVivas = 0, celMuertas = 0;
            bool generacionCongelada = false, esElPrimero = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\nMENÚ:\n");
            Console.ResetColor();
            Console.WriteLine("1) Elija esta opción si desea correr los turnos manualmente presionando SPACE\n\n2) Elija esta opción si desea correr automáticamente X cant. de turnos presionando la tecla 'X'\n");

            int opcion = LeerNum();
            EvaluarLimites(ref opcion, 1, 2);
            Console.Clear();

            switch (opcion)
            {
                case 1:
                    CorrerTurnoManual(matriz, matrizFutura, turno, nacimientos, muertes, nacimientosCongelados, muertesCongeladas, turnosCongelados, generacionCongelada, esElPrimero, celVivas, celMuertas);
                    break;
                case 2:
                    Console.WriteLine("¿Cuántos turnos desea correr?");
                    int numero = LeerNum();
                    while (numero <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Debe ingresar un número mayor a cero...");
                        Console.ResetColor();

                        numero = LeerNum();
                    }
                    Console.Clear();
                    CorrerXTurnos(matriz, numero, matrizFutura, turno, nacimientos, muertes, nacimientosCongelados, muertesCongeladas, turnosCongelados, generacionCongelada, esElPrimero, celVivas, celMuertas);
                    break;
            }
        }

        static void ImprimirGeneracionCongelada(bool generacionCongelada)
        {
            if (generacionCongelada)
            {
                Console.WriteLine("\nLA GENERACIÓN SE CONGELÓ");
                Console.WriteLine("No se han registrado cambios durante los dos últimos turnos");
                Console.WriteLine("\nPulse una tecla para volver al menú...\n");
                Console.ReadKey();
            }
        }

        static void EstadoCelular(int[,] matrizFutura, int[,] matrizActual, int i, int j, int vecinasVivas, ref int muertes, ref int nacimientos)
        {
            if (matrizActual[i, j] == 1) 
            {
                if (vecinasVivas <= 1 || vecinasVivas > 3)
                {
                    matrizFutura[i, j] = 0;
                    muertes++;
                }
                else if (vecinasVivas == 2 || vecinasVivas == 3)
                {
                    matrizFutura[i, j] = 1;
                }
            }
            else
            {
                if (vecinasVivas == 3)
                {
                    matrizFutura[i, j] = 1;
                    nacimientos++;
                }
            }
        }

        static void DeterminarGeneracionCongelada(ref bool esElPrimero, ref int nacimientosCongelados, ref int muertesCongeladas, ref int nacimientos, ref int muertes, ref int turnosCongelados, ref bool generacionCongelada)
        {
            if (esElPrimero)
            {
                nacimientosCongelados = nacimientos;
                muertesCongeladas = muertes;
                esElPrimero = false;
            }
            else
            {
                if (nacimientosCongelados == nacimientos && muertesCongeladas == muertes)
                {
                    turnosCongelados++;
                }
                else
                {
                    nacimientosCongelados = nacimientos;
                    muertesCongeladas = muertes;
                    turnosCongelados = 0;
                }
            }
            if (turnosCongelados == 2)
            {
                generacionCongelada = true;
            }

        }

        static void ImprimirGeneracionMuerta(int celVivas)
        {
            if (celVivas == 0)
            {
                Console.WriteLine("\n¡HAN MUERTO TODAS LAS CÉLULAS!");
                Console.WriteLine("\nPulse una tecla para volver al menú...\n");
                Console.ReadKey();
            }
        }

        static void GenerarMatriz(int[,] matrizFutura, int[,] matrizActual, ref int nacimientos, ref int muertes)
        {
            for (int i = 0; i < matrizActual.GetLength(0); i++)
            {
                for (int j = 0; j < matrizActual.GetLength(1); j++)
                {
                    int vecinasVivas = CelulasVecinas(i, j, matrizActual, FIL, COL);
                    EstadoCelular(matrizFutura, matrizActual, i, j, vecinasVivas, ref muertes, ref nacimientos);
                }
            }
        }

        static void EvaluarRespuesta(int[,] matriz, int turno, int muertes, int nacimientos)
        {
            Console.WriteLine("\n¿Desea guardar esta configuración inicial? (s/n)\n");
            string respuesta = Console.ReadLine();
            respuesta = respuesta.ToLower();
            while (respuesta != "n" && respuesta != "s")
            {
                Console.WriteLine("Responda si o no (s/n)...");
                respuesta = Console.ReadLine();
                respuesta = respuesta.ToLower();
            }

            if (respuesta == "s")
            {
                GuardarMatriz(matriz, turno, muertes, nacimientos);
            }
            Console.Clear();
        }

        static int[,] CargarMatriz(ref int turno, ref int nacimientos, ref int muertes)
        {
            Console.Write("Ingrese nombre del archivo: ");
            string nombreArchivo = Console.ReadLine();
            nombreArchivo += ".txt";
            while (!File.Exists(nombreArchivo))
            {
                Console.Write("El nombre no existe, ingrese otro: ");
                nombreArchivo = Console.ReadLine();
                nombreArchivo += ".txt";
            }

            FileStream file = new FileStream(nombreArchivo, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            char[] separadores = { ',', ';', '-', '|' };
            string[] linea;
            int fila, col;

            linea = sr.ReadLine().Split(separadores);

            fila = Convert.ToInt32(linea[0]);
            col = Int32.Parse(linea[1]);
            turno = Int32.Parse(linea[2]);
            nacimientos = Int32.Parse(linea[3]);
            muertes = Int32.Parse(linea[4]);

            int[,] matriz = new int[fila, col];

            for (int i = 0; i < fila; i++)
            {
                linea = sr.ReadLine().Split(separadores);
                for (int j = 0; j < col; j++)
                {
                    matriz[i, j] = Convert.ToInt32(linea[j]);
                }
            }

            sr.Close();
            file.Close();
            Console.Clear();
            Console.WriteLine("¡Carga exitosa!");
            Console.WriteLine("Presione tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
            return matriz;
        }

        static void GuardarMatriz(int[,] matriz, int turno, int muertes, int nacimientos)
        {
            Console.Clear();
            Console.ResetColor();
            Console.Write("\nIngrese el nombre del archivo a guardar: ");
            string nombreArchivo = Console.ReadLine();
            nombreArchivo += ".txt";
            while (File.Exists(nombreArchivo))
            {
                Console.Write("Ya existe ese nombre, ingrese otro: ");
                nombreArchivo = Console.ReadLine();
                nombreArchivo += ".txt";
            }
            FileStream file = new FileStream(nombreArchivo, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine(matriz.GetLength(0) + "," + matriz.GetLength(1) + "," + turno + "," + nacimientos + "," + muertes);
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    sw.Write(matriz[i, j]);
                    if (j != matriz.GetLength(1) - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.WriteLine();
            }
            sw.Close();
            file.Close();
            Console.Clear();
            Console.WriteLine("\n¡Guardado exitoso!");
            Console.WriteLine("Pulse una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void CorrerXTurnos(int[,] matrizOrigen, int numero, int[,] matrizFutura, int turno, int nacimientos, int muertes, int nacimientosCongelados, int muertesCongeladas, int turnosCongelados, bool generacionCongelada, bool esElPrimero, int celVivas, int celMuertas)
        {
            bool repetir = true;
            ImprimirMatriz(matrizOrigen, turno, muertes, nacimientos, ref celVivas, ref celMuertas);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Pulse X para comenzar a correr los turnos");
            Console.ResetColor();

            int[,] matrizActual = (int[,])matrizOrigen.Clone();

            ConsoleKeyInfo tecla = Console.ReadKey(true);           

            switch (tecla.Key)
            {
                case ConsoleKey.X:
                    do
                    {
                        Console.Clear();
                        GenerarMatriz(matrizFutura, matrizActual, ref nacimientos, ref muertes);
                        turno++;
                        ImprimirMatriz(matrizFutura, turno, muertes, nacimientos, ref celVivas, ref celMuertas);
                        matrizActual = (int[,])matrizFutura.Clone();
                        DeterminarGeneracionCongelada(ref esElPrimero, ref nacimientosCongelados, ref muertesCongeladas, ref nacimientos, ref muertes, ref turnosCongelados, ref generacionCongelada);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Pulse ENTER para guardar y salir");
                        Console.WriteLine("Pulse ESCAPE para volver al menú...\n");

                        if (Console.KeyAvailable)
                        {
                            tecla = Console.ReadKey(true);
                            if (tecla.Key == ConsoleKey.Escape)
                            {
                                repetir = false;
                            }
                            if (tecla.Key == ConsoleKey.Enter)
                            {
                                GuardarMatriz(matrizFutura, turno, muertes, nacimientos);
                                repetir = false;
                            }
                        }

                        Console.ResetColor();
                        System.Threading.Thread.Sleep(80);

                    } while (turno < numero && generacionCongelada == false && celVivas > 0 && repetir);

                    Console.ForegroundColor = ConsoleColor.Green;
                    ImprimirGeneracionCongelada(generacionCongelada);
                    ImprimirGeneracionMuerta(celVivas);
                    break;
            }
        }

        static void CorrerTurnoManual(int[,] matrizOrigen, int[,] matrizFutura, int turno, int nacimientos, int muertes, int nacimientosCongelados, int muertesCongeladas, int turnosCongelados, bool generacionCongelada, bool esElPrimero, int celVivas, int celMuertas)
        {
            bool repetir = true;
            ImprimirMatriz(matrizOrigen, turno, muertes, nacimientos, ref celVivas, ref celMuertas);
            int[,] matrizActual = (int[,])matrizOrigen.Clone();
            ConsoleKeyInfo tecla;

            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Pulse ESPACIO para avanzar un turno...");
                Console.WriteLine("Pulse ENTER para guardar y salir");
                Console.WriteLine("Pulse ESCAPE si desea volver al menú...\n");
                Console.ResetColor();

                tecla = Console.ReadKey(true);
                Console.Clear();

                if (tecla.Key == ConsoleKey.Enter && turno == 0)
                {
                    GuardarMatriz(matrizActual, turno, muertes, nacimientos);
                    repetir = false;
                }
                else if (tecla.Key == ConsoleKey.Enter && turno > 0)
                {
                    GuardarMatriz(matrizFutura, turno, muertes, nacimientos);
                    repetir = false;
                }

                GenerarMatriz(matrizFutura, matrizActual, ref nacimientos, ref muertes);
                turno++;
                ImprimirMatriz(matrizFutura, turno, muertes, nacimientos, ref celVivas, ref celMuertas);
                matrizActual = (int[,])matrizFutura.Clone();
                DeterminarGeneracionCongelada(ref esElPrimero, ref nacimientosCongelados, ref muertesCongeladas, ref nacimientos, ref muertes, ref turnosCongelados, ref generacionCongelada);

            } while (tecla.Key == ConsoleKey.Spacebar && tecla.Key != ConsoleKey.Escape && generacionCongelada == false && celVivas > 0 && repetir);

            Console.ForegroundColor = ConsoleColor.Green;
            ImprimirGeneracionCongelada(generacionCongelada);
            ImprimirGeneracionMuerta(celVivas);
        }

        static int CelulasVecinas(int f, int c, int[,] matriz, int FIL, int COL)
        {
            int celulasVivas = 0;

            // Abajo derecha
            if (f < FIL - 1 && c < COL - 1 && matriz[f + 1, c + 1] == 1) celulasVivas++;
            //Abajo
            if (f < FIL - 1 && matriz[f + 1, c] == 1) celulasVivas++;
            // Derecha
            if (c < COL - 1 && matriz[f, c + 1] == 1) celulasVivas++;
            // Arriba izq
            if (f > 0 && c > 0 && matriz[f - 1, c - 1] == 1) celulasVivas++;
            // Izquierda
            if (c > 0 && matriz[f, c - 1] == 1) celulasVivas++;
            // Arriba
            if (f > 0 && matriz[f - 1, c] == 1) celulasVivas++;
            // Abajo izquierda
            if (f < FIL - 1 && c > 0 && matriz[f + 1, c - 1] == 1) celulasVivas++;
            // Arriba derecha
            if (f > 0 && c < COL - 1 && matriz[f - 1, c + 1] == 1) celulasVivas++;

            return celulasVivas;
        }

        static int LeerNum()
        {
            bool error;
            int num;

            do
            {
                error = !int.TryParse(Console.ReadLine(), out num);
                if (error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("¡Sólo puede ingresar números!...\n\n");
                    Console.ResetColor();
                }
            }
            while (error);

            return num;
        }

        static void Main(string[] args)
        {
            bool salir = false;

            while (salir == false)
            {
                int[,] espacioCelular = new int[FIL, COL];
                int turno = 0, nacimientos = 0, muertes = 0;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("***SIMULADOR DE AUTÓMATA CELULAR***\n");
                Console.ResetColor();
                Console.WriteLine("Elija una opción:\n");
                Console.WriteLine("1) Cargar espacio Celular de manera MANUAL\n\n2) Cargar espacio Celular de manera ALEATORIA\n");
                Console.WriteLine("3) Cargar espacio Celular PREDETERMINADO\n\n4) Cargar configuración desde un archivo\n");
                Console.WriteLine("5) Salir\n");
                int opcion = LeerNum();
                EvaluarLimites(ref opcion, 1, 5);
                Console.Clear();

                switch (opcion)
                {
                    case 1:
                        CargarMatrizManual(espacioCelular);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("SU MATRIZ HA QUEDADO ASÍ:\n");
                        MostrarConfigInicial(espacioCelular);
                        EvaluarRespuesta(espacioCelular, turno, muertes, nacimientos);
                        MenuTurnos(espacioCelular, turno, nacimientos, muertes);
                        break;
                    case 2:
                        CargarMatrizAleatoria(espacioCelular);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("SU MATRIZ HA QUEDADO ASÍ:\n");
                        MostrarConfigInicial(espacioCelular);
                        EvaluarRespuesta(espacioCelular, turno, muertes, nacimientos);
                        MenuTurnos(espacioCelular, turno, nacimientos, muertes);
                        break;
                    case 3:                        
                        int[,] gliderGun = new int[FIL, COL];
                        gliderGun[1, 26] = 1;
                        gliderGun[2, 24] = 1;
                        gliderGun[2, 26] = 1;
                        gliderGun[3, 14] = 1;
                        gliderGun[3, 15] = 1;
                        gliderGun[3, 22] = 1;
                        gliderGun[3, 23] = 1;
                        gliderGun[4, 13] = 1;
                        gliderGun[4, 17] = 1;
                        gliderGun[4, 22] = 1;
                        gliderGun[4, 23] = 1;
                        gliderGun[4, 36] = 1;
                        gliderGun[4, 37] = 1;
                        gliderGun[5, 12] = 1;
                        gliderGun[5, 18] = 1;
                        gliderGun[5, 22] = 1;
                        gliderGun[5, 23] = 1;
                        gliderGun[5, 36] = 1;
                        gliderGun[5, 37] = 1;
                        gliderGun[6, 2] = 1;
                        gliderGun[6, 3] = 1;                       
                        gliderGun[6, 12] = 1;
                        gliderGun[6, 16] = 1;
                        gliderGun[6, 18] = 1;
                        gliderGun[6, 19] = 1;
                        gliderGun[6, 24] = 1;
                        gliderGun[6, 26] = 1;
                        gliderGun[7, 2] = 1;
                        gliderGun[7, 3] = 1;
                        gliderGun[7, 12] = 1;
                        gliderGun[7, 18] = 1;
                        gliderGun[7, 26] = 1;
                        gliderGun[8, 13] = 1;
                        gliderGun[8, 17] = 1;
                        gliderGun[9, 14] = 1;
                        gliderGun[9, 15] = 1;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\t\t\t\t\t***GOSPER GLIDER GUN***\n");
                        MostrarConfigInicial(gliderGun);
                        MenuTurnos(gliderGun, turno, nacimientos, muertes);
                        break;
                    case 4:
                        espacioCelular = CargarMatriz(ref turno, ref nacimientos, ref muertes);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("ESTA ES SU MATRIZ:\n");
                        MostrarConfigInicial(espacioCelular);
                        MenuTurnos(espacioCelular, turno, nacimientos, muertes);
                        break;
                    case 5:
                        salir = true;
                        break;
                }
                Console.Clear();
            }
        }
    }
}