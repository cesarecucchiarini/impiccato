﻿//VOID

string[,] ritornaParole()
{
    string[,] parole = new string[10, 51];
    string path = "paroleImpiccato.txt";
    StreamReader reader = new StreamReader(path);
    for(int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 51; j++)
        {
            parole[i, j] = reader.ReadLine().ToLower();
        }
    }
    return parole;
}

string[] scegliParola(string[,] parole)
{
    Random rnd = new Random();
    int t = tema(parole);
    int arg=0;
    string[] a = new string[2];
    int arg2 = 0;
    bool x = true;
    while (x)
    {
        for (int i = 1; i < parole.GetLength(1); i++)
        {
            if (parole[arg, i] != "")
            {
                x = false;
            }
        }
        if (x)
        {
            Console.WriteLine("Hai finito le parole del tema, scegline un altro");
            t = tema(parole);
            arg = t > -1 ? t : rnd.Next(10);
        }
    }
    while(arg2==0 || parole[arg, arg2] == "")
    {
        arg2 = rnd.Next(1, parole.GetLength(1));
    }
    if (arg == -1)
    { 
        arg = rnd.Next(10);
        a[0] = parole[arg, 0];
        a[1]= parole[arg,arg2];
    }
    else
    {
        a[0] = parole[arg, 0];
        a[1] = parole[arg,arg2];
    }
    parole[arg, arg2] = "";
    return a;
}

char decisione()
{
    Console.WriteLine("Vuoi un indizio (i), una lettera jolly (j), inserire una lettera (c), inserire la parola finale (f) o uscire (e)?");
    char s = Console.ReadLine().ToLower()[0];
    if (s == 'i')
    {
        Console.WriteLine("Hai scelto indizio");
        return '1';
    }
    else if (s == 'j')
    {
        Console.WriteLine("Hai scelto jolly");
        return '2';
    }
    else if (s == 'e')
    {
        Console.WriteLine("Hai scelto di uscire");
        return '4';
    }
    else if (s == 'f')
    {
        Console.WriteLine("Hai scelto di inserire la parola");
        return '5';
    }
    Console.WriteLine("Hai scelto inserimento");
    return '3';
}

void indizio(string[] scelta, char[] parola, ref int monete)
{
    Console.WriteLine($"Hai {monete} monete\nVuoi sapere la prima lettera per 10 monete (p), l'ultima lettera  per 5 (u) oppure il tema per 15 (t)?");
    char s = Console.ReadLine().ToLower()[0];
    if(s == 'p')
    {
        if (monete >= 10)
        {
            monete -= 10;
            parola[0] = scelta[1][0];
        }
        else
            Console.WriteLine("Non hai abbastanza monete");
    }
    else if(s == 'u')
    {
        if (monete >= 5)
        {
            monete -= 5;
            parola[parola.Length - 1] = scelta[1][scelta[1].Length - 1];
        }
        else
            Console.WriteLine("Non hai abbastanza monete");
    }
    else
    {
        if (monete >= 15)
        {
            monete -= 15;
            Console.WriteLine($"Il tema è \"{scelta[0]}\"");
        }
        else
            Console.WriteLine("Non hai abbastanza monete");
    }
}

void jolly(char[] parola, string[]scelta, ref bool jolly)
{
    if (jolly)
    {
        Random rnd = new Random();
        int let = rnd.Next(parola.Length);
        parola[let] = scelta[1][let];
        jolly = false;
    }
    else
        Console.WriteLine("Hai gia usato il jolly");
}

char inserimento(ref string provate)
{
    char s= '\0';
    while (!char.IsLetter(s) || provate.Contains(s))
    {
        Console.WriteLine("Dammi un carattere");
        s = Console.ReadLine().ToLower()[0];
        if (provate.Contains(s))
            Console.WriteLine("Carattere gia usato");
    }
    provate += $"{s} ";
    return s;
}

void aggiunta(char s, char[] parola, string[] scelta, ref int tentativi, ref int punti)
{
    if (scelta[1].Contains(s))
    {
        punti += 5;
        for(int i = 0; i < parola.Length; i++)
        {
            if (s == scelta[1][i])
                parola[i] = s;
        }
    }
    else
    {
        tentativi--;
        Console.WriteLine("La lettera non è presente nella parola");
    }
}

int tema(string[,]parole)
{
    int t = -1;
    Console.WriteLine("Vuoi scegliere il tema della parola (s) oppure avere un tema casuale (n)?");
    char s = Console.ReadLine().ToLower()[0];
    if(s == 's')
    {
        Console.WriteLine("Scegli il tema tra i seguenti (scrivi il numero)");
        for (int i = 0; i < parole.GetLength(0); i++) 
        {
            Console.WriteLine($"{i+1}) {parole[i, 0]}");
        }
        t=int.Parse(Console.ReadLine())-1;
    }
    if (t < -1 || t > 10)
        t = -1;
    return t;
}

bool inserimentoIntero(string[] scelta)
{
    Console.WriteLine("Dammi la parola completa");
    string p = Console.ReadLine().ToLower();
    if (p == scelta[1])
        return true;
    return false;
}
//MAIN

Console.WriteLine("Gioca all'impiccato, devi indovinare una parola segreta cercando di indovinarne i caratteri." +
    "Hai a disposizione dei tentativi, i quali indicano gli errori che puoi commettere, e le monete che ti servono a comprare indizi." +
    "Infine una volta per parola puoi usare un jolly che ti permetterà di trovare un carattere casuale della parola.\n\n");
//SCELTA MODALITA
Console.WriteLine("Scegli la modalità:\nfacile, 10 tentativi e 50 monete(f)\nmedia, 5 tentativi e 20 monete (m)\n" +
    "difficile, 3 tentativi e 10 monete (d)?");
string mod=Console.ReadLine();
int tentativi = 0, monete=0, punti=0;
string provate = "", paroleTrovate="";
bool j;
bool gioco = true;
string[] scelta = new string[2];
string parolaFinale;
while (gioco)
{
    j = true;
    switch (mod)
    {
        case "f":
            tentativi += 10;
            monete += 50;
            break;
        case "m":
            tentativi += 5;
            monete
                =+ 20;
            break;
        default:
            tentativi += 3;
            monete += 10;
            break;
    }
    //PAROLA DA INDOVINARE
    scelta = scegliParola(ritornaParole());
    char[] parola = new char[scelta[1].Length];
    for (int i = 0; i < parola.Length; i++)
    {
        parola[i] = '_';
    }


    //GIOCO
    while (tentativi > 0 && parola.Contains('_'))
    {
        parolaFinale = string.Concat(parola);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Parola: {parolaFinale} ({parolaFinale.Length})\nLettere usate: {provate}\nMonete rimaste: {monete}\nTentativi rimasti: {tentativi}\n");
        Console.ForegroundColor = ConsoleColor.White;
        //AZIONE
        char c = decisione();
        switch (c)
        {
            //INDIZIO
            case '1':
                indizio(scelta, parola, ref monete);
                break;

            //JOLLY
            case '2':
                jolly(parola, scelta, ref j);
                break;

            //PROVA
            case '3':
                c = inserimento(ref provate);
                aggiunta(c, parola, scelta, ref tentativi, ref punti);
                break;
            case '4':
                tentativi = 0;
                gioco = false;
                break;
            case '5':
                if (inserimentoIntero(scelta))
                {
                    Console.WriteLine("Parola corretta");
                    parolaFinale = "";
                    punti += 20;
                }
                else
                {
                    tentativi = 0;
                    Console.WriteLine("Hai sbagliato");
                }
                break;
        }
    }
    if (tentativi == 0&& gioco)
    {
        gioco = false;
        Console.WriteLine("Hai perso");
    }
    else if (tentativi == 0)
    {
        Console.WriteLine("Gioco terminato");
    }
    else
    {
        Console.WriteLine("Hai indovinato!!");
        paroleTrovate +=scelta[1]+" ";
    }
}
    Console.WriteLine($"Gioco finito, hai totalizzato {punti} punti\n" +
        $"Le parole trovate sono: {paroleTrovate}"); 