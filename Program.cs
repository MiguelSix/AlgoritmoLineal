using System;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;

namespace ALGORITMOLINEAL // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        //standard normal cumulative distribution function
        static double F(double x)
        {
            MathNet.Numerics.Distributions.Normal result = new MathNet.Numerics.Distributions.Normal();
            return result.CumulativeDistribution(x);
        }
        static void Main(string[] args)
        {
            List<double> numAleatorios = new List<double>();
            StreamWriter log;
            log = new StreamWriter("C:\\Users\\wachi\\OneDrive\\Escritorio\\SIMULACION\\AlgoritmoLineal\\file.log");
            log.AutoFlush = true;

            int duplicados(int cont)
            {
                List<double> result = numAleatorios.GroupBy(x => x)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key)
                        .ToList();
                if (result.Count() > 0)
                {
                    string cosa = (String.Join(", ", result));
                    Console.WriteLine("Se encontro un numero duplicado en la iteracion <" + cont + ">, dicho numero es: " + cosa);
                    numAleatorios.RemoveAt(numAleatorios.Count() - 1);
                    return 1;
                }
                return 0;
            }

            void print()
            {
                int i = 1;
                log.WriteLine("Lista de numeros: ");
                foreach (double a in numAleatorios)
                {
                    log.WriteLine("<" + i + ">,  Valor: " + a);
                    i++;
                }
                Console.WriteLine("Total de numeros: " + numAleatorios.Count());
            }

            void pruebaDeMedias(double zAlfa)
            {
                double media = 0;
                foreach (double a in numAleatorios)
                {
                    media = media + a;
                }
                media = media / numAleatorios.Count();
                log.WriteLine("\n\n\t\tPrueba de medias");
                Console.WriteLine("\n\n\t\tPrueba de medias");
                log.WriteLine("La media del conjunto es: " + media);
                Console.WriteLine("La media del conjunto es: " + media);

                double cotaInf = 0, cotaSup = 0;
                double mult = 1 / Math.Sqrt(numAleatorios.Count() * 12);
                cotaInf = (0.5) - (zAlfa * mult);
                cotaSup = (0.5) + (zAlfa * mult);
                log.WriteLine("La cota inferior es: " + cotaInf + ", y la cota superior es: " + cotaSup);
                Console.WriteLine("La cota inferior es: " + cotaInf + ", y la cota superior es: " + cotaSup);
                if (media > cotaInf && media < cotaSup)
                {
                    log.WriteLine("El conjunto ha pasado la prueba de medias!!");
                    Console.WriteLine("El conjunto ha pasado la prueba de medias!!");
                }
                else
                {
                    log.WriteLine("El conjunto no ha pasado la prueba de medias!!");
                    Console.WriteLine("El conjunto no ha pasado la prueba de medias!!");
                }
            }

            void pruebaVarianza(int nvlConfianza, double chiNormal, double chiComplemento)
            {
                double media = 0;
                log.WriteLine("\n\n\t\tPrueba de varianza");
                Console.WriteLine("\n\n\t\tPrueba de varianza");
                foreach (double a in numAleatorios)
                {
                    media = media + a;
                }
                media = media / numAleatorios.Count();
                double var = 0;

                foreach (double a in numAleatorios)
                {
                    var = var + (Math.Pow((a - media), 2));
                }
                var = var / (numAleatorios.Count() - 1);
                log.WriteLine("La varianza es : " + var);
                Console.WriteLine("La varianza es : " + var);

                var CHI = new ChiSquared(numAleatorios.Count() - 1);
                //Usamos como parametro el nivel de confianza
                chiComplemento = CHI.InverseCumulativeDistribution((0.05 / 2));
                chiNormal = CHI.InverseCumulativeDistribution((1 - 0.05 / 2));

                //Limites
                double limInf = 0, limSup = 0;

                limInf = chiNormal / (12 * (numAleatorios.Count() - 1));

                limSup = chiComplemento / (12 * (numAleatorios.Count() - 1));

                log.WriteLine("El limite inferior es: " + limInf + ", y el superior es: " + limSup);
                Console.WriteLine("El limite inferior es: " + limInf + ", y el superior es: " + limSup);
                if (var > limInf && var < limSup)
                {
                    log.WriteLine("El conjunto ha pasado la prueba de varianza!!");
                    Console.WriteLine("El conjunto ha pasado la prueba de varianza!!");
                    log.WriteLine("La varianza del conjunto se encuentra entre los limites de aceptacion, por lo que no podemos rechazar que el conjunto tiene una varianza de 1/12, con un nivel de aceptacion del " + nvlConfianza + "%");
                    Console.WriteLine("La varianza del conjunto se encuentra entre los limites de aceptacion, por lo que no podemos rechazar que el conjunto tiene una varianza de 1/12, con un nivel de aceptacion del " + nvlConfianza + "%");
                }
                else
                {
                    log.WriteLine("El conjunto no ha pasado la prueba de varianza!!");
                    Console.WriteLine("El conjunto ha pasado la prueba de varianza!!");
                    log.WriteLine("El conjunto no ha pasado la prueba de varianza!!");
                    Console.WriteLine("El conjunto no ha pasado la prueba de varianza!!");
                }
            }

            bool IsBetween(double inf, double sup, double num)
            {
                if (num >= inf && num <= sup) return true;
                return false;
            }

            void pruebaUniformidad(double chiTabla)
            {
                log.WriteLine("\n\n\t\tPrueba de Uniformidad");
                Console.WriteLine("\n\n\t\tPrueba de Uniformidad");
                double m = Math.Ceiling(Math.Sqrt(numAleatorios.Count()));
                double anchoClase = 1 / m;
                double inf = 0, sup = 0;
                int cont = 0, i = 1;
                double E = Math.Ceiling(numAleatorios.Count() / m);
                double chiCalculado = 0;

                var CHI = new ChiSquared(m - 1);
                //Usamos como parametro emnivel de confianza
                chiTabla = CHI.InverseCumulativeDistribution(0.95);

                while (inf <= 1 && i <= m)
                {
                    cont = 0;
                    sup = i * anchoClase;
                    if (sup > 1) sup = 1;
                    //Console.WriteLine("\n");
                    //Console.Write("<" + i + ">, " + "(" + inf + "-" + sup + "]: ");
                    log.WriteLine("\n");
                    log.Write("<" + i + ">, " + "(" + inf + "-" + sup + "]: ");
                    foreach (double a in numAleatorios)
                    {
                        if (IsBetween(inf, sup, a)) cont++;
                    }
                    //Console.Write("" + cont);
                    log.Write("" + cont);
                    chiCalculado = chiCalculado + Math.Pow((E - cont), 2);
                    inf = sup;
                    i++;
                }
                chiCalculado = chiCalculado / E;
                Console.WriteLine("\nEl valor de Chi calculado es de: " + chiCalculado);
                log.WriteLine("\n\nEl valor de Chi calculado es de: " + chiCalculado);

                if (chiCalculado < chiTabla)
                {
                    Console.WriteLine("No podemos rechazar que el conjunto sigue una distribucion uniforme");
                    log.WriteLine("No podemos rechazar que el conjunto sigue una distribucion uniforme");
                    Console.WriteLine("El conjunto ha pasado la prueba de uniformidad!!");
                    log.WriteLine("El conjunto ha pasado la prueba de uniformidad!!");
                }
                else
                {
                    Console.WriteLine("El conjunto no sigue una distribucion uniforme");
                    log.WriteLine("El conjunto no sigue una distribucion uniforme");
                }
            }

            void pruebaIndependencia(double zTablaNeg, double zTablaPos, int conf)
            {
                log.WriteLine("\n\n\t\tPrueba de Independencia");
                Console.WriteLine("\n\n\t\tPrueba de Independencia");
                List<int> secuenciaUnosCeros = new List<int>();
                int corridasObservadas = 0;
                int nCeros = 0, nUnos = 0;
                double media, zeta;
                double varianza;
                //Llenar de ceros y unos
                foreach (double d in numAleatorios)
                {
                    if (d >= 0.5) secuenciaUnosCeros.Add(1);
                    else secuenciaUnosCeros.Add(0);
                }

                for (int i = 0; i < secuenciaUnosCeros.Count() - 1; i++)
                {
                    if (secuenciaUnosCeros[i] == 0 && secuenciaUnosCeros[i + 1] == 1) corridasObservadas++;
                    if (secuenciaUnosCeros[i] == 1 && secuenciaUnosCeros[i + 1] == 0) corridasObservadas++;
                }

                foreach (int i in secuenciaUnosCeros)
                {
                    if (i == 0) nCeros++;
                    if (i == 1) nUnos++;
                }

                //Calculos
                double n = numAleatorios.Count();
                media = ((2 * nCeros * nUnos) / n) + 0.5;


                double num = (2 * nCeros * nUnos) * (2 * nCeros * nUnos - n);
                double denom = (Math.Pow(n, 2)) * (n - 1);
                varianza = num / denom;

                double arriba = corridasObservadas - media;
                double abajo = Math.Sqrt(varianza);

                zeta = arriba / abajo;


                log.WriteLine("Datos:");
                log.WriteLine("Corridas observadas: " + corridasObservadas);
                log.WriteLine("Numero de ceros: " + nCeros);
                log.WriteLine("Numero de unos: " + nUnos);
                log.WriteLine("Valor de la media: " + media);
                log.WriteLine("Valor de la varianza: " + varianza);
                log.WriteLine("Valor de la zeta: " + zeta);

                Console.WriteLine("Datos:");
                Console.WriteLine("Corridas observadas: " + corridasObservadas);
                Console.WriteLine("Numero de ceros: " + nCeros);
                Console.WriteLine("Numero de unos: " + nUnos);
                Console.WriteLine("Valor de la media: " + media);
                Console.WriteLine("Valor de la varianza: " + varianza);
                Console.WriteLine("Valor de la zeta: " + zeta);

                if (zeta > zTablaNeg && zeta < zTablaPos)
                {
                    Console.WriteLine("No podemos rechazar que el conjunto es independiente, con un nivel de confianza del " + conf + "%");
                    log.WriteLine("No podemos rechazar que el conjunto es independiente, con un nivel de confianza del " + conf + "%");
                    Console.WriteLine("El conjunto ha pasado la prueba de independencia!!");
                    log.WriteLine("El conjunto ha pasado la prueba de independencia!!");
                }
                else
                {
                    Console.WriteLine("El conjunto no es independiente");
                    log.WriteLine("El conjunto no es independiente");
                }
            }

            void generarNumerosAleatorios(double x, int k, double g, double c)
            {
                double m = Math.Pow(2, g);
                double a = 1 + 4 * k;
                double r = 0;
                int i = 0;
                int cosa = duplicados(i);
                while (cosa != 1)
                {
                    r = x / (m - 1);
                    numAleatorios.Add(r);
                    double val = (((a * x) + c));
                    x = val % m;
                    i++;
                    cosa = duplicados(i);
                }
            }

            void unDado(int corr)
            {
                log.WriteLine("\n\nSimulacion de la tirada de un dado");

                List<double> corrida = new List<double>();
                List<int> numeros = new List<int>();
                int n = 50, i = 0, corridasLimit = 1;
                double mediaDmedias = 0;
                List<double> medias = new List<double>();
                while (corridasLimit <= corr)
                {

                    log.WriteLine("Corrida <" + corridasLimit + ">:\n");

                    corrida.RemoveRange(0, corrida.Count());
                    numeros.RemoveRange(0, corrida.Count());

                    while (corrida.Count() != 50)
                    {
                        if (corrida.Count() == 50)
                        {
                            break;
                        }
                        corrida.Add(numAleatorios[i]);
                        i++;
                    }

                    foreach (double d in corrida)
                    {
                        if (d >= 0 && d < 0.166666666)
                        {
                            numeros.Add(1);
                        }
                        else if (d >= 0.16666666 && d < 0.33333333)
                        {
                            numeros.Add(2);
                        }
                        else if (d >= 0.33333333 && d < 0.5)
                        {
                            numeros.Add(3);
                        }
                        else if (d >= 0.5 && d < 0.66666666)
                        {
                            numeros.Add(4);
                        }
                        else if (d >= 0.66666666 && d < 0.83333333)
                        {
                            numeros.Add(5);
                        }
                        else if (d >= 0.833333333 && d <= 1)
                        {
                            numeros.Add(6);
                        }
                    }

                    //Media
                    float media = 0;

                    foreach (int d in numeros)
                    {
                        media = media + d;
                    }
                    media = media / n;
                    mediaDmedias = mediaDmedias + media;
                    medias.Add(media);

                    numeros.Sort();

                    //Imprimir las listas
                    log.WriteLine("Lista de numeros: ");
                    log.Write("[");
                    foreach (int x in numeros)
                    {
                        log.Write(x + ",");
                    }
                    log.Write("]\n");
                    log.WriteLine("\nLista de numeros ri:");
                    log.Write("[");
                    foreach (double x in corrida)
                    {
                        log.Write(x + ",");
                    }
                    log.Write("]\n");
                    log.WriteLine("\nLa media de la corrida es: " + media);

                    //Moda
                    int unos = 0, dos = 0, tres = 0, cuatro = 0, cinco = 0, seis = 0;
                    int moda = 0;

                    foreach (int x in numeros)
                    {
                        if (x == 1) unos++;
                        if (x == 2) dos++;
                        if (x == 3) tres++;
                        if (x == 4) cuatro++;
                        if (x == 5) cinco++;
                        if (x == 6) seis++;
                    }
                    int max = 0, aux = 0;
                    while (aux < 6)
                    {
                        if (unos > max)
                        {
                            max = unos;
                            moda = 1;
                        }
                        else if (dos > max)
                        {
                            max = dos;
                            moda = 2;
                        }
                        else if (tres > max)
                        {
                            max = tres;
                            moda = 3;
                        }
                        else if (cuatro > max)
                        {
                            max = cuatro;
                            moda = 4;
                        }
                        else if (cinco > max)
                        {
                            max = cinco;
                            moda = 5;
                        }
                        else if (seis > max)
                        {
                            max = seis;
                            moda = 6;
                        }
                        aux++;
                    }
                    log.WriteLine("La moda de la corrida es: " + moda);

                    //Desviacion estandar
                    double desv = 0;
                    foreach (int x in numeros)
                    {
                        desv = desv + Math.Pow((x - media), 2);
                    }
                    desv = desv / n;
                    log.WriteLine("La desviacion estandar de la corrida es: " + desv);

                    //Mediana

                    double mediana = 0;
                    while (numeros.Count() != 2)
                    {
                        numeros.RemoveAt(0);
                        numeros.RemoveAt(numeros.Count() - 1);
                    }
                    foreach (int x in numeros)
                    {
                        mediana = mediana + x;
                    }
                    mediana = mediana / 2;
                    log.WriteLine("La mediana es: " + mediana);

                    double varianza = 0;
                    varianza = Math.Pow(desv, 2);
                    log.WriteLine("La varianza es: " + varianza);

                    corridasLimit++;
                }
                mediaDmedias = mediaDmedias / corr;
                log.WriteLine("\n\n\nIntervalo de confianza");
                log.WriteLine("La media de medias es: " + mediaDmedias + "\n");
                double desvEstandarMedias = 0;
                foreach (double x in medias)
                {
                    desvEstandarMedias = desvEstandarMedias + Math.Pow((x - mediaDmedias), 2);
                }
                desvEstandarMedias = desvEstandarMedias / corr;
                log.WriteLine("La desviacion estandar de las medias es: " + desvEstandarMedias);
                double int1 = 0;
                double int2 = 0;
                int1 = mediaDmedias - desvEstandarMedias / Math.Sqrt(corr) * (2.262);
                int2 = mediaDmedias + desvEstandarMedias / Math.Sqrt(corr) * (2.262);
                log.WriteLine("El intervalo de confianza es: [" + int1 + "," + int2 + "]\n\n");
            }

            void teoriaDeColas(int numPersonas, int corridas, double costoHora, double costoHorasExtra, double costoEsperaCamion, double costoAlmacen)
            {
                log.WriteLine("\n\nSimulacion de la teoria de colas");
                //Console.WriteLine("\n\nSimulacion de la teoria de colas");
                if (numPersonas < 3 || numPersonas > 7)
                {
                    Console.WriteLine("No existe un equipo con ese numero de personas");
                    log.WriteLine("No existe un equipo con ese numero de personas");
                    return;
                }
                //Variables iguales para todos los casos
                DateTime hora = new DateTime(2022, 10, 07, 23, 0, 0);
                DateTime horaLimite = new DateTime(2022, 10, 08, 07, 30, 0);
                DateTime horaComida = new DateTime(2022, 10, 08, 03, 00, 0);
                int i = 0;
                int camionesEnEspera = 0;
                double rCamionesEnEspera = 0;
                int corridaActual = 1;
                double mediaDCostos = 0;
                List<double> costos = new List<double>();
                costos.RemoveRange(0, costos.Count());
                double int1 = 0;
                double int2 = 0;
                double desvEstandarCostos = 0;

                //Costos
                int hrsTiempoNormal = 8;
                double hrsTiempoExtra = 0;
                double hrsTiempoEspera = 0;
                double hrsAlmacen = 0;
                double costoTotal = 0;
                bool comida = false;

                int inversaCamiones(double r)
                {
                    if (r >= 0 && r < 0.5)
                    {
                        return 0;
                    }
                    else if (r >= 0.5 && r < 0.75)
                    {
                        return 1;
                    }
                    else if (r >= 0.75 && r < 0.9)
                    {
                        return 2;
                    }
                    else if (r >= 0.9 && r <= 1)
                    {
                        return 3;
                    }
                    return 0;
                }

                int inversaTiempoLlegada(double r)
                {
                    if (r >= 0 && r < 0.02)
                    {
                        return 20;
                    }
                    else if (r >= 0.02 && r < 0.1)
                    {
                        return 25;
                    }
                    else if (r >= 0.1 && r < 0.22)
                    {
                        return 30;
                    }
                    else if (r >= 0.22 && r <= 0.47)
                    {
                        return 35;
                    }
                    else if (r >= 0.47 && r <= 0.67)
                    {
                        return 40;
                    }
                    else if (r >= 0.67 && r <= 0.82)
                    {
                        return 45;
                    }
                    else if (r >= 0.82 && r <= 0.92)
                    {
                        return 50;
                    }
                    else if (r >= 0.92 && r <= 0.97)
                    {
                        return 55;
                    }
                    else if (r >= 0.97 && r <= 1)
                    {
                        return 60;
                    }
                    return 0;
                }

                switch (numPersonas)
                {
                    case 3:
                        int inversaTiempoServicioTres(double r)
                        {
                            if (r >= 0 && r < 0.05)
                            {
                                return 20;
                            }
                            else if (r >= 0.05 && r < 0.15)
                            {
                                return 25;
                            }
                            else if (r >= 0.15 && r < 0.35)
                            {
                                return 30;
                            }
                            else if (r >= 0.35 && r <= 0.6)
                            {
                                return 35;
                            }
                            else if (r >= 0.6 && r <= 0.72)
                            {
                                return 40;
                            }
                            else if (r >= 0.72 && r <= 0.82)
                            {
                                return 45;
                            }
                            else if (r >= 0.82 && r <= 0.9)
                            {
                                return 50;
                            }
                            else if (r >= 0.9 && r <= 0.96)
                            {
                                return 55;
                            }
                            else if (r >= 0.96 && r <= 1)
                            {
                                return 60;
                            }
                            return 0;
                        }
                        i = 0;
                        camionesEnEspera = 0;
                        rCamionesEnEspera = 0;
                        corridaActual = 1;

                        //Costos
                        hrsTiempoNormal = 8;
                        hrsTiempoExtra = 0;
                        hrsTiempoEspera = 0;
                        hrsAlmacen = 0;
                        costoTotal = 0;

                        double imprimeCosto()
                        {
                            costoTotal = ((hrsTiempoNormal * costoHora) * 3) + ((hrsTiempoExtra * costoHorasExtra) * 3) + (hrsTiempoEspera * costoEsperaCamion) + (hrsAlmacen * costoAlmacen);
                            log.WriteLine("Costo de horas normales: $" + (hrsTiempoNormal * costoHora) * 3);
                            log.WriteLine("Costo de horas extra: $" + (hrsTiempoExtra * costoHorasExtra) * 3);
                            log.WriteLine("Costo de tiempo de espera en los camiones: $" + hrsTiempoEspera * costoEsperaCamion);
                            log.WriteLine("Costo de mantener el almacen: $" + hrsAlmacen * costoAlmacen);
                            log.WriteLine("Costo total: $" + Math.Round(costoTotal, 4));
                            costos.Add(costoTotal);
                            return costoTotal;
                        }

                        while (corridaActual <= corridas)
                        {
                            //Comida
                            comida = false;

                            log.WriteLine("\n\nCorrida: " + corridaActual);
                            rCamionesEnEspera = numAleatorios[i];
                            i++;
                            camionesEnEspera = inversaCamiones(rCamionesEnEspera);
                            log.WriteLine("Camiones en espera: " + camionesEnEspera + "\nPSE: " + rCamionesEnEspera);
                            log.WriteLine("#PSE" + "\t\tH.Llegada" + "\t\tHora.Ent.Des" + "\t#PSE" + "\t\tT.Des" + "\tH.Salida" + "\tT.Espera");
                            llegaCamion(camionesEnEspera);

                            mediaDCostos = mediaDCostos + imprimeCosto();
                            corridaActual++;
                        }

                        void llegaCamion(int camionesEnEspera)
                        {
                            double PSE1;
                            double PSE2;
                            DateTime horaLlegada = hora;
                            DateTime horaEntradaDescarga;
                            DateTime horaSalidaCamion = hora;
                            double tiempoEsperaTotal = 0;
                            int tiempoEspera;
                            int tiempoDescarga;
                            int camiones;

                            if (camionesEnEspera == 1)
                            {
                                camiones = 1;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioTres(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        DateTime temp = horaSalidaCamion;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioTres(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 2)
                            {
                                camiones = 2;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioTres(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioTres(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 3)
                            {
                                camiones = 3;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioTres(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioTres(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }

                            while (horaLlegada < horaLimite)
                            {
                                PSE1 = numAleatorios[i];
                                i++;
                                PSE2 = numAleatorios[i];
                                i++;
                                horaLlegada = horaLlegada.AddMinutes(inversaTiempoLlegada(PSE1));
                                //Nose si esto jala bien
                                if (horaLlegada > horaLimite)
                                {
                                    i--;
                                    i--;
                                    break;
                                }
                                //Si llega a la hora de comida, se espera 30 minutos
                                if (horaLlegada == horaComida && comida == false)
                                {
                                    log.WriteLine("El personal comienza a comer a las:" + horaLlegada.TimeOfDay);
                                    horaLlegada = horaLlegada.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaLlegada.TimeOfDay);
                                    comida = true;
                                }
                                //
                                if (horaSalidaCamion < horaLlegada)
                                {
                                    horaEntradaDescarga = horaLlegada;
                                }
                                else
                                {
                                    horaEntradaDescarga = horaSalidaCamion;
                                }
                                horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioTres(PSE2));
                                //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                if ((horaSalidaCamion >= horaComida) && (comida == false))
                                {
                                    comida = true;
                                    DateTime temp = horaSalidaCamion;
                                    log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                    horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioTres(PSE2);
                                    log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                                    continue;
                                }
                                tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                tiempoDescarga = inversaTiempoServicioTres(PSE2);
                                log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                            }
                            hrsTiempoExtra = (double)horaSalidaCamion.Subtract(horaLimite).TotalHours;
                            //Console.WriteLine("Tiempo extra: " + hrsTiempoExtra);
                            hrsTiempoEspera = tiempoEsperaTotal / 60;
                            hrsAlmacen = (double)horaSalidaCamion.Subtract(hora).TotalHours;
                        }
                        //Console.WriteLine("\n\nIntervalo de confianza: ");
                        log.WriteLine("\n\nIntervalo de confianza: ");
                        mediaDCostos = mediaDCostos / corridas;
                        log.WriteLine("\nMedia de costos: " + Math.Round(mediaDCostos, 4));
                        desvEstandarCostos = 0;
                        foreach (double x in costos)
                        {
                            desvEstandarCostos = desvEstandarCostos + Math.Pow((x - mediaDCostos), 2);
                        }
                        //desvEstandarCostos = Statistics.StandardDeviation(costos);
                        desvEstandarCostos = desvEstandarCostos / corridas;
                        desvEstandarCostos = Math.Sqrt(desvEstandarCostos);
                        log.WriteLine("La desviacion estandar de los costos es: " + desvEstandarCostos);
                        //Console.WriteLine("La desviacion estandar de los costos es: " + desvEstandarCostos);
                        desvEstandarCostos = Statistics.StandardDeviation(costos);
                        //Console.WriteLine("La desviacion estandar de los costos modificcada es: " + desvEstandarCostos);
                        int1 = 0;
                        int2 = 0;
                        int1 = mediaDCostos - desvEstandarCostos / Math.Sqrt(corridas) * (2.262);
                        int2 = mediaDCostos + desvEstandarCostos / Math.Sqrt(corridas) * (2.262);
                        log.WriteLine("El intervalo de confianza es: [" + int1 + "," + int2 + "]\n\n");
                        break;
                    case 4:
                        int inversaTiempoServicioCuatro(double r)
                        {
                            if (r >= 0 && r < 0.05)
                            {
                                return 15;
                            }
                            else if (r >= 0.05 && r < 0.20)
                            {
                                return 20;
                            }
                            else if (r >= 0.20 && r < 0.40)
                            {
                                return 25;
                            }
                            else if (r >= 0.40 && r <= 0.6)
                            {
                                return 30;
                            }
                            else if (r >= 0.6 && r <= 0.75)
                            {
                                return 35;
                            }
                            else if (r >= 0.75 && r <= 0.87)
                            {
                                return 40;
                            }
                            else if (r >= 0.87 && r <= 0.95)
                            {
                                return 45;
                            }
                            else if (r >= 0.95 && r <= 0.99)
                            {
                                return 50;
                            }
                            else if (r >= 0.99 && r <= 1)
                            {
                                return 55;
                            }
                            return 0;
                        }
                        i = 0;
                        camionesEnEspera = 0;
                        rCamionesEnEspera = 0;
                        corridaActual = 1;

                        //Costos
                        hrsTiempoNormal = 8;
                        hrsTiempoExtra = 0;
                        hrsTiempoEspera = 0;
                        hrsAlmacen = 0;
                        costoTotal = 0;

                        while (corridaActual <= corridas)
                        {
                            //Comida
                            comida = false;

                            log.WriteLine("\n\nCorrida: " + corridaActual);
                            Console.WriteLine("\n\nCorrida: " + corridaActual);

                            rCamionesEnEspera = numAleatorios[i];
                            camionesEnEspera = inversaCamiones(rCamionesEnEspera);
                            i++;
                            log.WriteLine("Camiones en espera: " + camionesEnEspera + "\nPSE: " + rCamionesEnEspera);
                            log.WriteLine("#PSE" + "\t\tH.Llegada" + "\t\tHora.Ent.Des" + "\t#PSE" + "\t\tT.Des" + "\tH.Salida" + "\tT.Espera");
                            llegaCamionCuatro(camionesEnEspera);
                            mediaDCostos = mediaDCostos + imprimeCostoCuatro();
                            corridaActual++;
                        }
                        //log.WriteLine("#PSE" + "\t\tHora de llegada" + "\t\tHora de entrada a descarga" + "\t#PSE" + "\t\tTiempo de descarga" + "\tHora de salida del camion" + "\tTiempo de espera" );
                        double imprimeCostoCuatro()
                        {
                            costoTotal = ((hrsTiempoNormal * costoHora) * 4) + ((hrsTiempoExtra * costoHorasExtra) * 4) + (hrsTiempoEspera * costoEsperaCamion) + (hrsAlmacen * costoAlmacen);
                            log.WriteLine("Costo de horas normales: $" + (hrsTiempoNormal * costoHora) * 4);
                            log.WriteLine("Costo de horas extra: $" + (hrsTiempoExtra * costoHorasExtra) * 4);
                            log.WriteLine("Costo de tiempo de espera en los camiones: $" + hrsTiempoEspera * costoEsperaCamion);
                            log.WriteLine("Costo de mantener el almacen: $" + hrsAlmacen * costoAlmacen);
                            log.WriteLine("Costo total: $" + Math.Round(costoTotal, 4));
                            return costoTotal;
                        }

                        void llegaCamionCuatro(int camionesEnEspera)
                        {
                            double PSE1;
                            double PSE2;
                            DateTime horaLlegada = hora;
                            DateTime horaEntradaDescarga;
                            DateTime horaSalidaCamion = hora;
                            double tiempoEsperaTotal = 0;
                            int tiempoEspera;
                            int tiempoDescarga;
                            int camiones;

                            if (camionesEnEspera == 1)
                            {
                                camiones = 1;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCuatro(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCuatro(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 2)
                            {
                                camiones = 2;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCuatro(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCuatro(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 3)
                            {
                                camiones = 3;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCuatro(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCuatro(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }

                            while (horaLlegada < horaLimite)
                            {
                                PSE1 = numAleatorios[i];
                                i++;
                                PSE2 = numAleatorios[i];
                                i++;
                                horaLlegada = horaLlegada.AddMinutes(inversaTiempoLlegada(PSE1));
                                //Nose si esto jala bien
                                if (horaLlegada > horaLimite)
                                {
                                    i--;
                                    i--;
                                    break;
                                }
                                if (horaLlegada == horaComida)
                                {
                                    log.WriteLine("El personal comienza a comer a las:" + horaLlegada.TimeOfDay);
                                    horaLlegada = horaLlegada.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaLlegada.TimeOfDay);
                                    comida = true;
                                }
                                //
                                if (horaSalidaCamion < horaLlegada)
                                {
                                    horaEntradaDescarga = horaLlegada;
                                }
                                else
                                {
                                    horaEntradaDescarga = horaSalidaCamion;
                                }
                                horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCuatro(PSE2));
                                //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                if ((horaSalidaCamion >= horaComida) && (comida == false))
                                {
                                    comida = true;
                                    DateTime temp = horaSalidaCamion;
                                    log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                    horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCuatro(PSE2);
                                    log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                                    continue;
                                }
                                tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                tiempoDescarga = inversaTiempoServicioCuatro(PSE2);
                                log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                            }
                            hrsTiempoExtra = (double)horaSalidaCamion.Subtract(horaLimite).TotalHours;
                            //Console.WriteLine("Tiempo extra: " + hrsTiempoExtra);
                            hrsTiempoEspera = tiempoEsperaTotal / 60;
                            hrsAlmacen = (double)horaSalidaCamion.Subtract(hora).TotalHours;
                        }
                        //Console.WriteLine("\n\nIntervalo de confianza: ");
                        log.WriteLine("\n\nIntervalo de confianza: ");
                        mediaDCostos = mediaDCostos / corridas;
                        log.WriteLine("\nMedia de costos: " + Math.Round(mediaDCostos, 4));
                        desvEstandarCostos = 0;
                        foreach (double x in costos)
                        {
                            desvEstandarCostos = desvEstandarCostos + Math.Pow((x - mediaDCostos), 2);
                        }
                        desvEstandarCostos = desvEstandarCostos / corridas;
                        log.WriteLine("La desviacion estandar de los costos es: " + desvEstandarCostos);
                        int1 = 0;
                        int2 = 0;
                        int1 = mediaDCostos - desvEstandarCostos / Math.Sqrt(corridas) * (2.262);
                        int2 = mediaDCostos + desvEstandarCostos / Math.Sqrt(corridas) * (2.262);
                        log.WriteLine("El intervalo de confianza es: [" + int1 + "," + int2 + "]\n\n");
                        break;
                    case 5:
                        int inversaTiempoServicioCinco(double r)
                        {
                            if (r >= 0 && r < 0.1)
                            {
                                return 10;
                            }
                            else if (r >= 0.1 && r < 0.28)
                            {
                                return 15;
                            }
                            else if (r >= 0.28 && r < 0.5)
                            {
                                return 20;
                            }
                            else if (r >= 0.5 && r <= 0.68)
                            {
                                return 25;
                            }
                            else if (r >= 0.68 && r <= 0.78)
                            {
                                return 30;
                            }
                            else if (r >= 0.78 && r <= 0.86)
                            {
                                return 35;
                            }
                            else if (r >= 0.86 && r <= 0.92)
                            {
                                return 40;
                            }
                            else if (r >= 0.92 && r <= 0.97)
                            {
                                return 45;
                            }
                            else if (r >= 0.97 && r <= 1)
                            {
                                return 50;
                            }
                            return 0;
                        }
                        i = 0;
                        camionesEnEspera = 0;
                        rCamionesEnEspera = 0;
                        corridaActual = 1;

                        //Costos
                        hrsTiempoNormal = 8;
                        hrsTiempoExtra = 0;
                        hrsTiempoEspera = 0;
                        hrsAlmacen = 0;
                        costoTotal = 0;

                        while (corridaActual <= corridas)
                        {
                            //Comida
                            comida = false;

                            log.WriteLine("\n\nCorrida: " + corridaActual);
                            Console.WriteLine("\n\nCorrida: " + corridaActual);

                            rCamionesEnEspera = numAleatorios[i];
                            camionesEnEspera = inversaCamiones(rCamionesEnEspera);
                            i++;
                            log.WriteLine("Camiones en espera: " + camionesEnEspera + "\nPSE: " + rCamionesEnEspera);
                            log.WriteLine("#PSE" + "\t\tH.Llegada" + "\t\tHora.Ent.Des" + "\t#PSE" + "\t\tT.Des" + "\tH.Salida" + "\tT.Espera");
                            llegaCamionCinco(camionesEnEspera);
                            mediaDCostos = mediaDCostos + imprimeCostoCinco();
                            corridaActual++;
                        }
                        //log.WriteLine("#PSE" + "\t\tHora de llegada" + "\t\tHora de entrada a descarga" + "\t#PSE" + "\t\tTiempo de descarga" + "\tHora de salida del camion" + "\tTiempo de espera" );
                        double imprimeCostoCinco()
                        {
                            costoTotal = ((hrsTiempoNormal * costoHora) * 5) + ((hrsTiempoExtra * costoHorasExtra) * 5) + (hrsTiempoEspera * costoEsperaCamion) + (hrsAlmacen * costoAlmacen);
                            log.WriteLine("Costo de horas normales: $" + (hrsTiempoNormal * costoHora) * 5);
                            log.WriteLine("Costo de horas extra: $" + (hrsTiempoExtra * costoHorasExtra) * 5);
                            log.WriteLine("Costo de tiempo de espera en los camiones: $" + hrsTiempoEspera * costoEsperaCamion);
                            log.WriteLine("Costo de mantener el almacen: $" + hrsAlmacen * costoAlmacen);
                            log.WriteLine("Costo total: $" + Math.Round(costoTotal, 4));
                            return costoTotal;
                        }

                        void llegaCamionCinco(int camionesEnEspera)
                        {
                            double PSE1;
                            double PSE2;
                            DateTime horaLlegada = hora;
                            DateTime horaEntradaDescarga;
                            DateTime horaSalidaCamion = hora;
                            double tiempoEsperaTotal = 0;
                            int tiempoEspera;
                            int tiempoDescarga;
                            int camiones;

                            if (camionesEnEspera == 1)
                            {
                                camiones = 1;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCinco(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCinco(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 2)
                            {
                                camiones = 2;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCinco(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCinco(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 3)
                            {
                                camiones = 3;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCinco(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCinco(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }

                            while (horaLlegada < horaLimite)
                            {
                                PSE1 = numAleatorios[i];
                                i++;
                                PSE2 = numAleatorios[i];
                                i++;
                                horaLlegada = horaLlegada.AddMinutes(inversaTiempoLlegada(PSE1));
                                //Nose si esto jala bien
                                if (horaLlegada > horaLimite)
                                {
                                    i--;
                                    i--;
                                    break;
                                }
                                if (horaLlegada == horaComida)
                                {
                                    log.WriteLine("El personal comienza a comer a las:" + horaLlegada.TimeOfDay);
                                    horaLlegada = horaLlegada.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaLlegada.TimeOfDay);
                                    comida = true;
                                }
                                //
                                if (horaSalidaCamion < horaLlegada)
                                {
                                    horaEntradaDescarga = horaLlegada;
                                }
                                else
                                {
                                    horaEntradaDescarga = horaSalidaCamion;
                                }
                                horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCinco(PSE2));
                                //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                if ((horaSalidaCamion >= horaComida) && (comida == false))
                                {
                                    comida = true;
                                    DateTime temp = horaSalidaCamion;
                                    log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                    horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCinco(PSE2);
                                    log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                                    continue;
                                }
                                tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                tiempoDescarga = inversaTiempoServicioCinco(PSE2);

                                log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                            }
                            hrsTiempoExtra = (double)horaSalidaCamion.Subtract(horaLimite).TotalHours;
                            //Console.WriteLine("Tiempo extra: " + hrsTiempoExtra);
                            hrsTiempoEspera = tiempoEsperaTotal / 60;
                            hrsAlmacen = (double)horaSalidaCamion.Subtract(hora).TotalHours;
                        }
                        //Console.WriteLine("\n\nIntervalo de confianza: ");
                        log.WriteLine("\n\nIntervalo de confianza: ");
                        mediaDCostos = mediaDCostos / corridas;
                        log.WriteLine("\nMedia de costos: " + Math.Round(mediaDCostos, 4));
                        desvEstandarCostos = 0;
                        foreach (double x in costos)
                        {
                            desvEstandarCostos = desvEstandarCostos + Math.Pow((x - mediaDCostos), 2);
                        }
                        desvEstandarCostos = desvEstandarCostos / corridas;
                        log.WriteLine("La desviacion estandar de las medias es: " + desvEstandarCostos);
                        int1 = 0;
                        int2 = 0;
                        int1 = mediaDCostos - desvEstandarCostos / Math.Sqrt(corridas) * (2.262);
                        int2 = mediaDCostos + desvEstandarCostos / Math.Sqrt(corridas) * (2.262);
                        log.WriteLine("El intervalo de confianza es: [" + int1 + "," + int2 + "]\n\n");
                        break;
                    case 6:
                        int inversaTiempoServicioSeis(double r)
                        {
                            if (r >= 0 && r < 0.12)
                            {
                                return 5;
                            }
                            else if (r >= 0.12 && r < 0.27)
                            {
                                return 10;
                            }
                            else if (r >= 0.27 && r < 0.53)
                            {
                                return 15;
                            }
                            else if (r >= 0.53 && r <= 0.68)
                            {
                                return 20;
                            }
                            else if (r >= 0.68 && r <= 0.8)
                            {
                                return 25;
                            }
                            else if (r >= 0.8 && r <= 0.88)
                            {
                                return 30;
                            }
                            else if (r >= 0.88 && r <= 0.94)
                            {
                                return 35;
                            }
                            else if (r >= 0.94 && r <= 0.98)
                            {
                                return 40;
                            }
                            else if (r >= 0.98 && r <= 1)
                            {
                                return 45;
                            }
                            return 0;
                        }
                        i = 0;
                        camionesEnEspera = 0;
                        rCamionesEnEspera = 0;
                        corridaActual = 1;

                        //Costos
                        hrsTiempoNormal = 8;
                        hrsTiempoExtra = 0;
                        hrsTiempoEspera = 0;
                        hrsAlmacen = 0;
                        costoTotal = 0;

                        while (corridaActual <= corridas)
                        {
                            //Comida
                            comida = false;

                            log.WriteLine("\n\nCorrida: " + corridaActual);
                            Console.WriteLine("\n\nCorrida: " + corridaActual);

                            rCamionesEnEspera = numAleatorios[i];
                            camionesEnEspera = inversaCamiones(rCamionesEnEspera);
                            i++;
                            log.WriteLine("Camiones en espera: " + camionesEnEspera + "\nPSE: " + rCamionesEnEspera);
                            log.WriteLine("#PSE" + "\t\tH.Llegada" + "\t\tHora.Ent.Des" + "\t#PSE" + "\t\tT.Des" + "\tH.Salida" + "\tT.Espera");
                            llegaCamionSeis(camionesEnEspera);
                            mediaDCostos = mediaDCostos + imprimeCostoSeis();
                            corridaActual++;
                        }
                        //log.WriteLine("#PSE" + "\t\tHora de llegada" + "\t\tHora de entrada a descarga" + "\t#PSE" + "\t\tTiempo de descarga" + "\tHora de salida del camion" + "\tTiempo de espera" );
                        double imprimeCostoSeis()
                        {
                            costoTotal = ((hrsTiempoNormal * costoHora) * 6) + ((hrsTiempoExtra * costoHorasExtra) * 6) + (hrsTiempoEspera * costoEsperaCamion) + (hrsAlmacen * costoAlmacen);
                            log.WriteLine("Costo de horas normales: $" + (hrsTiempoNormal * costoHora) * 6);
                            log.WriteLine("Costo de horas extra: $" + (hrsTiempoExtra * costoHorasExtra) * 6);
                            log.WriteLine("Costo de tiempo de espera en los camiones: $" + hrsTiempoEspera * costoEsperaCamion);
                            log.WriteLine("Costo de mantener el almacen: $" + hrsAlmacen * costoAlmacen);
                            log.WriteLine("Costo total: $" + Math.Round(costoTotal, 4));
                            return costoTotal;
                        }

                        void llegaCamionSeis(int camionesEnEspera)
                        {
                            double PSE1;
                            double PSE2;
                            DateTime horaLlegada = hora;
                            DateTime horaEntradaDescarga;
                            DateTime horaSalidaCamion = hora;
                            double tiempoEsperaTotal = 0;
                            int tiempoEspera;
                            int tiempoDescarga;
                            int camiones;

                            if (camionesEnEspera == 1)
                            {
                                camiones = 1;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioSeis(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioSeis(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 2)
                            {
                                camiones = 2;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioSeis(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioSeis(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 3)
                            {
                                camiones = 3;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = numAleatorios[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioSeis(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioSeis(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }

                            while (horaLlegada < horaLimite)
                            {
                                PSE1 = numAleatorios[i];
                                i++;
                                PSE2 = numAleatorios[i];
                                i++;
                                horaLlegada = horaLlegada.AddMinutes(inversaTiempoLlegada(PSE1));
                                //Nose si esto jala bien
                                if (horaLlegada > horaLimite)
                                {
                                    i--;
                                    i--;
                                    break;
                                }
                                if (horaLlegada == horaComida)
                                {
                                    log.WriteLine("El personal comienza a comer a las:" + horaLlegada.TimeOfDay);
                                    horaLlegada = horaLlegada.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaLlegada.TimeOfDay);
                                    comida = true;

                                }
                                //
                                if (horaSalidaCamion < horaLlegada)
                                {
                                    horaEntradaDescarga = horaLlegada;
                                }
                                else
                                {
                                    horaEntradaDescarga = horaSalidaCamion;
                                }
                                horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioSeis(PSE2));
                                //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                if ((horaSalidaCamion >= horaComida) && (comida == false))
                                {
                                    comida = true;
                                    DateTime temp = horaSalidaCamion;
                                    log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                    horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioSeis(PSE2);
                                    log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                                    continue;
                                }
                                tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                tiempoDescarga = inversaTiempoServicioSeis(PSE2);

                                log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                            }
                            hrsTiempoExtra = (double)horaSalidaCamion.Subtract(horaLimite).TotalHours;
                            //Console.WriteLine("Tiempo extra: " + hrsTiempoExtra);
                            hrsTiempoEspera = tiempoEsperaTotal / 60;
                            hrsAlmacen = (double)horaSalidaCamion.Subtract(hora).TotalHours;
                        }
                        //Console.WriteLine("\n\nIntervalo de confianza: ");
                        log.WriteLine("\n\nIntervalo de confianza: ");
                        mediaDCostos = mediaDCostos / corridas;
                        log.WriteLine("\nMedia de costos: " + Math.Round(mediaDCostos, 4));
                        desvEstandarCostos = 0;
                        foreach (double x in costos)
                        {
                            desvEstandarCostos = desvEstandarCostos + Math.Pow((x - mediaDCostos), 2);
                        }
                        desvEstandarCostos = desvEstandarCostos / corridas;
                        log.WriteLine("La desviacion estandar de las medias es: " + desvEstandarCostos);
                        int1 = 0;
                        int2 = 0;
                        int1 = mediaDCostos - desvEstandarCostos / Math.Sqrt(corridas) * (2.262);
                        int2 = mediaDCostos + desvEstandarCostos / Math.Sqrt(corridas) * (2.262);
                        log.WriteLine("El intervalo de confianza es: [" + int1 + "," + int2 + "]\n\n");
                        break;
                }
            }

            List<double> generadorVariablesPoisson(int limiteVariables, double lambda)
            {
                log.WriteLine("\n\nGenerador de variables aleatorias usando el generador de Poisson:\n\n");
                int n = 0;
                double tPrima = 0, t = 1;
                int i = 0;
                int variablesGeneradas = 0;
                List<double> variables = new List<double>();
                while (variablesGeneradas < limiteVariables)
                {
                    tPrima = t * numAleatorios[i];
                    if (tPrima >= Math.Exp(-lambda))
                    {
                        n++;
                        t = tPrima;
                    }
                    else
                    {
                        variablesGeneradas++;
                        //Console.WriteLine("Variable <" + variablesGeneradas + "> generada en la iteracion [" + (i) + "]" + "\tValor = " + n);
                        //log.WriteLine("Variable <" + variablesGeneradas + "> generada en la iteracion [" + (i) + "]" + "\tValor = " + n);
                        log.WriteLine(n);
                        variables.Add(n);
                        n = 0;
                        t = 1;
                    }
                    i++;
                }
                return variables;
            }

            List<double> generadorVariablesNormales(int numeroDeVariables, double desvEstandar, double media)
            {
                log.WriteLine("\n\nGenerador de variables aleatorias usando el generador Normal:\n\n");
                int variablesGeneradas = 0, i = 0;
                List<double> numerosUsados = new List<double>();
                List<double> variables = new List<double>();
                double N = 0;
                while (variablesGeneradas < numeroDeVariables)
                {
                    numerosUsados.RemoveRange(0, numerosUsados.Count());
                    for (int j = 0; j < 12; j++)
                    {
                        N = N + (numAleatorios[i]);
                        numerosUsados.Add(numAleatorios[i]);
                        i++;
                    }
                    N = N - 6;
                    N = N * desvEstandar + media;
                    if (N >= 0)
                    {
                        variablesGeneradas++;
                        //log.WriteLine("Variable <" + (variablesGeneradas) + "> generada \tValor = " + N);
                        log.WriteLine(N);
                        variables.Add(N);
                    }
                    else
                    {
                        //log.WriteLine("<" + N + "> no se genero");
                    }
                    N = 0;
                }
                return variables;
            }

            bool IsBetweenv2(double inf, double sup, double num)
            {
                if (num >= inf && num < sup) return true;
                return false;
            }

            void pruebaChiCuadradaPoisson(List<double> variables)
            {
                double menor = variables.Min();
                double mayor = variables.Max();
                double media = Statistics.Mean(variables);
                double desvEstandar = Statistics.StandardDeviation(variables);
                double varianza = Statistics.Variance(variables);
                double n = variables.Count();
                double m = Math.Sqrt(n);
                double rango = mayor - menor;
                double anchoClase = rango / m;
                anchoClase = Math.Ceiling(anchoClase);
                media = Math.Round(media, 0);
                desvEstandar = Math.Round(desvEstandar, 2);
                m = Math.Ceiling(m);

                Console.WriteLine("\n\nPrueba de Chi Cuadrada para Poisson:\n\n");
                Console.WriteLine("Datos de la muestra:");
                Console.WriteLine("Menor: " + menor);
                Console.WriteLine("Mayor: " + mayor);
                Console.WriteLine("Media: " + media);
                Console.WriteLine("Desviacion Estandar: " + desvEstandar);
                Console.WriteLine("Varianza: " + varianza);
                Console.WriteLine("Numero de variables: " + n);
                Console.WriteLine("Numero de clases: " + m);
                Console.WriteLine("Rango: " + rango);
                Console.WriteLine("Ancho de clase: " + anchoClase);
                Console.WriteLine("Intervalos: " + m);

                log.WriteLine("\n\nPrueba de Chi Cuadrada para Poisson:\n\n");
                log.WriteLine("Datos de la muestra:");
                log.WriteLine("Menor: " + menor);
                log.WriteLine("Mayor: " + mayor);
                log.WriteLine("Media: " + media);
                log.WriteLine("Desviacion Estandar: " + desvEstandar);
                log.WriteLine("Varianza: " + varianza);
                log.WriteLine("Numero de variables: " + n);
                log.WriteLine("Numero de clases: " + m);
                log.WriteLine("Rango: " + rango);
                log.WriteLine("Ancho de clase: " + anchoClase);
                log.WriteLine("Intervalos: " + m);

                //Generar Intervalos
                Console.WriteLine("\nIntervalo \tOi \tx \tp(x) \t\tEi \tError");
                log.WriteLine("\nIntervalo \t\tOi \tx \tp(x) \t\tEi \tError");
                int inter = 0;
                double frecuenciaObservada = 0;
                double limiteInferior = 0;
                double limiteSuperior = 0;
                double x, px, ei, error, sumaProb = 0;
                double pxAnterior = 0;
                double chiCalculado = 0;
                //Usamos como parametro los grados de libertad, que es igual a la cantidad de intervalos - 1 - el numero de parametros
                var CHI = new ChiSquared(m - 1);
                //Usamos como parametro el nivel de confianza
                double chiTabla = CHI.InverseCumulativeDistribution(0.95);

                //Usamos como parametro lambda, que es igual a la media
                var POISSON = new Poisson(media);

                while (inter < m)
                {
                    frecuenciaObservada = 0;
                    limiteInferior = 0 + (inter * anchoClase);
                    if (inter == m - 1)
                    {
                        limiteSuperior = 1000;
                    }
                    else
                    {
                        limiteSuperior = limiteInferior + anchoClase;
                    }
                    foreach (double var in variables)
                    {
                        if (IsBetweenv2(limiteInferior, limiteSuperior, var))
                        {
                            frecuenciaObservada++;
                        }
                    }
                    if (inter == m - 1)
                    {
                        x = 0;
                        px = 1 - sumaProb;
                        sumaProb = sumaProb + px;
                        ei = n * px;
                        error = Math.Pow((ei - frecuenciaObservada), 2) / ei;
                        chiCalculado = chiCalculado + error;
                        Console.WriteLine("[" + Math.Round(limiteInferior, 2) + "," + Math.Round(limiteSuperior, 2) + "  ] \t" + frecuenciaObservada + " \t" + x + " \t" + Math.Round(px, 5) + "  \t" + Math.Round(ei, 2) + " \t" + Math.Round(error, 2));
                        log.WriteLine("[" + Math.Round(limiteInferior, 2) + " - " + Math.Round(limiteSuperior, 2) + "] \t" + frecuenciaObservada + " \t" + x + " \t" + Math.Round(px, 5) + "  \t" + Math.Round(ei, 2) + " \t" + Math.Round(error, 2));
                    }
                    else
                    {
                        x = limiteSuperior;
                        px = POISSON.CumulativeDistribution(x);
                        px = px - pxAnterior;
                        pxAnterior = pxAnterior + px;
                        sumaProb = sumaProb + px;
                        ei = n * px;
                        error = Math.Pow((ei - frecuenciaObservada), 2) / ei;
                        chiCalculado = chiCalculado + error;
                        Console.WriteLine("[" + Math.Round(limiteInferior, 2) + "," + Math.Round(limiteSuperior, 2) + "  ] \t" + frecuenciaObservada + " \t" + x + " \t" + Math.Round(px, 5) + "  \t" + Math.Round(ei, 2) + " \t" + Math.Round(error, 2));
                        log.WriteLine("[" + Math.Round(limiteInferior, 2) + " - " + Math.Round(limiteSuperior, 2) + "] \t\t" + frecuenciaObservada + " \t" + x + " \t" + Math.Round(px, 5) + "  \t" + Math.Round(ei, 2) + " \t" + Math.Round(error, 2));
                    }
                    inter++;
                }
                log.WriteLine("\nChi Calculado: " + Math.Round(chiCalculado, 2));
                Console.WriteLine("Chi Calculado: " + Math.Round(chiCalculado, 2));
                Console.WriteLine("Chi Tabla: " + Math.Round(chiTabla, 2));
                log.WriteLine("Chi Tabla: " + chiTabla);
                if (chiCalculado < chiTabla)
                {
                    Console.WriteLine("Se ha pasado la prueba");
                    log.WriteLine("Se ha pasado la prueba\n\n");
                }
                else
                {
                    Console.WriteLine("No se ha pasado la prueba");
                    log.WriteLine("No se ha pasado la prueba\n\n");
                }

            }

            void pruebaChiCuadradaNormal(List<double> variables)
            {
                double menor = variables.Min();
                double mayor = variables.Max();
                double media = Statistics.Mean(variables);
                double desvEstandar = Statistics.StandardDeviation(variables);
                double varianza = Statistics.Variance(variables);
                double n = variables.Count();
                double m = Math.Sqrt(n);
                double rango = mayor - menor;
                double anchoClase = rango / m;
                anchoClase = Math.Round(anchoClase, 2);
                media = Math.Round(media, 0);
                desvEstandar = Math.Round(desvEstandar, 2);
                m = Math.Ceiling(m);

                //Si hay parametros, hay que sobreescribir los valores de media y desviacion estandar, sino pues se comenta xd 
                /*
                media = 10;
                desvEstandar = 6.5;
                */


                Console.WriteLine("\n\nPrueba de Chi Cuadrada normal:\n");
                Console.WriteLine("Datos de la muestra:");
                Console.WriteLine("Menor: " + menor);
                Console.WriteLine("Mayor: " + mayor);
                Console.WriteLine("Media: " + media);
                Console.WriteLine("Desviacion Estandar: " + desvEstandar);
                Console.WriteLine("Varianza: " + varianza);
                Console.WriteLine("Numero de variables: " + n);
                Console.WriteLine("Numero de clases: " + m);
                Console.WriteLine("Rango: " + rango);
                Console.WriteLine("Ancho de clase: " + anchoClase);
                Console.WriteLine("Intervalos: " + m);

                log.WriteLine("\n\nPrueba de Chi Cuadrada Normal:\n\n");
                log.WriteLine("Datos de la muestra:");
                log.WriteLine("Menor: " + menor);
                log.WriteLine("Mayor: " + mayor);
                log.WriteLine("Media: " + media);
                log.WriteLine("Desviacion Estandar: " + desvEstandar);
                log.WriteLine("Varianza: " + varianza);
                log.WriteLine("Numero de variables: " + n);
                log.WriteLine("Numero de clases: " + m);
                log.WriteLine("Rango: " + rango);
                log.WriteLine("Ancho de clase: " + anchoClase);
                log.WriteLine("Intervalos: " + m);

                //Generar Intervalos
                Console.WriteLine("\nIntervalo \tOi \tx \tz \tp(z) \tp(x) \tEi \tError");
                log.WriteLine("\nIntervalo \t\tOi \tx \tz \tp(z) \tp(x) \tEi \tError");
                double pxAnterior = 0;
                int inter = 0;
                double frecuenciaObservada = 0;
                double limiteInferior = 0;
                double limiteSuperior = 0;
                double x, z, pz, pxActual, ei, error, sumaProb = 0;
                double chiCalculado = 0;

                //Usamos como parametro los grados de libertad, que es igual a la cantidad de intervalos - 1 - el numero de parametros
                var CHI = new ChiSquared(m - 1);
                //Usamos como parametro el nivel de confianza
                double chiTabla = CHI.InverseCumulativeDistribution(0.95);

                while (inter < m)
                {
                    frecuenciaObservada = 0;
                    limiteInferior = 0 + (inter * anchoClase);
                    if (inter == m - 1)
                    {
                        limiteSuperior = 10000;
                    }
                    else
                    {
                        limiteSuperior = limiteInferior + anchoClase;
                    }
                    foreach (double var in variables)
                    {
                        if (IsBetweenv2(limiteInferior, limiteSuperior, var))
                        {
                            frecuenciaObservada++;
                        }
                    }
                    if (inter == m - 1)
                    {
                        x = 0;
                        z = (x - media) / desvEstandar;
                        z = Math.Round(z, 2);
                        z = 0;
                        pz = F(z);
                        pz = 0;
                        pxActual = 1 - sumaProb;
                        sumaProb = sumaProb + pxActual;
                        ei = n * pxActual;
                        error = Math.Pow((ei - frecuenciaObservada), 2) / ei;
                        chiCalculado = chiCalculado + error;
                    }
                    else
                    {
                        x = limiteSuperior;
                        z = (x - media) / desvEstandar;
                        z = Math.Round(z, 2);
                        pz = F(z);
                        pxActual = pz;
                        pxActual = pxActual - pxAnterior;
                        pxAnterior = pz;
                        sumaProb = sumaProb + pxActual;
                        ei = n * pxActual;
                        error = Math.Pow((ei - frecuenciaObservada), 2) / ei;
                        chiCalculado = chiCalculado + error;
                        //Console.WriteLine("[" + Math.Round(limiteInferior, 2) + "," + Math.Round(limiteSuperior, 2) + "]\t" + frecuenciaObservada + "\t" + Math.Round(x, 2) + "\t" + Math.Round(z, 2) + "\t" + Math.Round(pz, 2) + "\t" + Math.Round(pxActual, 2) + "\t" + Math.Round(ei, 2) + "\t" + Math.Round(error, 2));
                    }
                    Console.WriteLine("[" + Math.Round(limiteInferior, 2) + "," + Math.Round(limiteSuperior, 2) + "] \t" + frecuenciaObservada + " \t" + Math.Round(x, 2) + " \t" + Math.Round(z, 2) + " \t" + Math.Round(pz, 2) + " \t" + Math.Round(pxActual, 2) + " \t" + Math.Round(ei, 2) + " \t" + Math.Round(error, 2));
                    log.WriteLine("[" + Math.Round(limiteInferior, 2) + "," + Math.Round(limiteSuperior, 2) + "  ] \t" + frecuenciaObservada + " \t" + Math.Round(x, 2) + " \t" + Math.Round(z, 2) + " \t" + Math.Round(pz, 2) + " \t" + Math.Round(pxActual, 2) + " \t" + Math.Round(ei, 2) + " \t" + Math.Round(error, 2));
                    inter++;
                }
                log.WriteLine("\nChi Calculado: " + Math.Round(chiCalculado, 2));
                Console.WriteLine("Chi Calculado: " + Math.Round(chiCalculado, 2));
                Console.WriteLine("Chi Tabla: " + chiTabla);
                log.WriteLine("Chi Tabla: " + chiTabla);
                if (chiCalculado < chiTabla)
                {
                    Console.WriteLine("Se ha pasado la prueba");
                    log.WriteLine("Se ha pasado la prueba");
                }
                else
                {
                    Console.WriteLine("No se ha pasado la prueba");
                    log.WriteLine("No se ha pasado la prueba");
                }
            }

            numAleatorios.RemoveRange(0, numAleatorios.Count());

            //x, k, g, c
            //generarNumerosAleatorios(6, 15, 13, 8191);
            generarNumerosAleatorios(5, 7, 11, 2047);
            print();
            //Pruebas
            pruebaDeMedias(1.96);
            pruebaVarianza(95, 0, 0);
            pruebaUniformidad(0);
            pruebaIndependencia(-1.96, 1.96, 95);

            unDado(10);
            //num de trabajadores, corridas, costo horas normales, costo horas extras, costo horas de espera, costo de almacen
            //teoriaDeColas(3, 10, 25.0, 37.5, 100, 500);
            //generadorVariablesPoisson(60, 17);
            //generadorVariablesNormales(100, 6.5, 10);
            //pruebaChiCuadradaNormal(generadorVariablesNormales(100, 6.5, 10));
            //pruebaChiCuadradaPoisson(generadorVariablesPoisson(45,5.07));

        }
    }
}
