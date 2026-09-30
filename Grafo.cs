using System.Text;

public class Grafo
{
    private readonly Dictionary<string, List<string>> adyacencia = new();

    public void AgregarArista(string origen, string destino)
    {
        if (!adyacencia.ContainsKey(origen))
            adyacencia[origen] = new List<string>();

        if (!adyacencia.ContainsKey(destino))
            adyacencia[destino] = new List<string>();

        adyacencia[origen].Add(destino);
        adyacencia[destino].Add(origen);
    }

    public void CargarDesdeArchivo(string ruta)
    {
        foreach (string linea in File.ReadAllLines(ruta))
        {
            string[] partes = linea.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 2)
                AgregarArista(partes[0], partes[1]);
        }
    }

    public void MostrarAdyacencia()
    {
        foreach (var par in adyacencia)
            Console.WriteLine($"{par.Key} -> {string.Join(", ", par.Value)}");
    }

    public void MostrarVertices()
    {
        Console.WriteLine($"Vértices ({adyacencia.Count}): {string.Join(", ", adyacencia.Keys)}");
    }

    public void MostrarAristas()
    {
        Console.WriteLine("Aristas:");

        foreach (var par in adyacencia)
        {
            foreach (string vecino in par.Value)
            {
                if (string.Compare(par.Key, vecino) < 0)
                    Console.WriteLine($"  {par.Key} - {vecino}");
            }
        }
    }

    public void ConsultarVertice(string vertice)
    {
        if (adyacencia.TryGetValue(vertice, out var vecinos))
            Console.WriteLine($"{vertice}: grado {vecinos.Count}, vecinos: {string.Join(", ", vecinos)}");
        else
            Console.WriteLine($"El vértice {vertice} no existe.");
    }

    public void GenerarSvg(string ruta)
    {
        const int ancho = 500;
        const int alto = 500;
        const int radioGrafo = 180;
        const int radioNodo = 32;

        List<string> nombres = adyacencia.Keys.ToList();
        Dictionary<string, (int x, int y)> pos = new();

        for (int i = 0; i < nombres.Count; i++)
        {
            double ang = 2 * Math.PI * i / nombres.Count - Math.PI / 2;
            pos[nombres[i]] = (
                (int)(ancho / 2 + radioGrafo * Math.Cos(ang)),
                (int)(alto / 2 + radioGrafo * Math.Sin(ang))
            );
        }

        StringBuilder sb = new();
        sb.AppendLine($"<svg xmlns='http://www.w3.org/2000/svg' width='{ancho}' height='{alto}'>");
        sb.AppendLine("<rect width='100%' height='100%' fill='white'/>");

        foreach (var par in adyacencia)
        {
            foreach (string vecino in par.Value)
            {
                if (string.Compare(par.Key, vecino) < 0)
                    sb.AppendLine($"<line x1='{pos[par.Key].x}' y1='{pos[par.Key].y}' x2='{pos[vecino].x}' y2='{pos[vecino].y}' stroke='black' stroke-width='2'/>");
            }
        }

        foreach (string n in nombres)
        {
            sb.AppendLine($"<circle cx='{pos[n].x}' cy='{pos[n].y}' r='{radioNodo}' fill='#cfe8ff' stroke='black' stroke-width='2'/>");
            sb.AppendLine($"<text x='{pos[n].x}' y='{pos[n].y + 5}' font-size='13' text-anchor='middle' font-family='sans-serif'>{n}</text>");
        }

        sb.AppendLine("</svg>");
        File.WriteAllText(ruta, sb.ToString());
    }
}