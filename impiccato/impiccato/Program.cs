//VOID

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
    int arg=tema(parole);
    Random rnd = new Random();
    string[] a = new string[2];
    if (arg == -1)
    {       
        arg = rnd.Next(10);
        a[0] = parole[arg, 0];
        a[1]= parole[arg, rnd.Next(1, parole.GetLength(1))];
    }
    else
    {
        a[0] = parole[arg, 0];
        a[1] = parole[arg, rnd.Next(1, parole.GetLength(1))];
    }
    return a;
}

char decisione()
{
    Console.WriteLine("Vuoi un indizio (i), una lettera jolly (j) o inserire una lettera (c)?");
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
    Console.WriteLine("Hai scelto inserimento");
    return '3';
}

void indizio(string[] scelta, char[] parola, ref int monete)
{
    Console.WriteLine("Vuoi sapere la prima lettera per 10 monete (p), l'ultima lettera  per 5 (u) oppure il tema per 15 (t)?");
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

void aggiunta(char s, char[] parola, string[] scelta, ref int tentativi)
{
    if (scelta[1].Contains(s))
    {
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
//MAIN

Console.WriteLine("Gioca all'impiccato, devi indovinare una parola segreta cercando di indovinarne i caratteri." +
    "Hai a disposizione dei tentativi, i quali indicano gli errori che puoi commettere, e le monete che ti servono a comprare indizi." +
    "Infine una volta per partita puoi usare un jolly che ti permetterà di trovare un carattere casuale della parola.\n\n");
//SCELTA MODALITA
Console.WriteLine("Scegli la modalità:\nfacile, 10 tentativi e 50 monete(f)\nmedia, 5 tentativi e 20 monete (m)\n" +
    "difficile, 3 tentativi e 10 monete (d)?");
string mod=Console.ReadLine();
int tentativi = 0, monete=0;
string provate = "";
bool j=true;
switch (mod)
{
    case "f":
        tentativi = 10;
        monete = 50;
        break;
    case "m":
        tentativi = 5;
        monete = 20;
        break;
    default:
        tentativi = 3;
        monete = 10;
        break;
}


//PAROLA DA INDOVINARE
string[] scelta = scegliParola(ritornaParole());
char[] parola = new char[scelta[1].Length];
string parolaFinale;
for (int i = 0; i < parola.Length; i++)
{
    parola[i] = '_';
}


//GIOCO
while (tentativi > 0 && parola.Contains('_'))
{
    parolaFinale = string.Concat(parola);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Parola: {parolaFinale}\nLettere usate: {provate}\nMonete rimaste: {monete}\nTentativi rimasti: {tentativi}\n");
    Console.ForegroundColor= ConsoleColor.White;
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
        default:
            c = inserimento(ref provate);
            aggiunta(c, parola, scelta, ref tentativi);
            break;
    }
}
if (tentativi > 0)
{
    Console.WriteLine("HAI VINTOOO!!!");
}
else
    Console.WriteLine("Hai perso, la parola era" + scelta[1]);