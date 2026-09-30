using System.Diagnostics;

static void ProcesarGrafo(string titulo, string archivo, string verticeConsulta)
{
    Console.WriteLine($"=== {titulo} ===");

    Stopwatch sw = Stopwatch.StartNew();
    Grafo grafo = new();
    grafo.CargarDesdeArchivo(archivo);
    grafo.MostrarAdyacencia();
    grafo.MostrarVertices();
    grafo.MostrarAristas();
    grafo.ConsultarVertice(verticeConsulta);
    grafo.GenerarSvg($"{Path.GetFileNameWithoutExtension(archivo)}.svg");
    sw.Stop();

    Console.WriteLine($"Tiempo de ejecución {titulo.ToLowerInvariant()}: {sw.Elapsed.TotalMilliseconds:F4} ms");
}

ProcesarGrafo("GRAFO 1", "grafo1.txt", "D");
Console.WriteLine();
ProcesarGrafo("GRAFO 2", "grafo2.txt", "Puyo");