using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    // Variável GameObject para o prefab do peixe, int para o número de peixes, array de GameObjects para todos os peixes, e Vector3 (área para instanciar os peixes).
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;
    // Variáveis float para o cardume.
    [Header("Configurações do Cardume")]
    [Range(0f, 5f)]
    public float minSpeed;
    [Range(0f, 5f)]
    public float maxSpeed;
    [Range(1f, 10f)]
    public float neighbourDistance;
    [Range(0f, 5f)]
    public float rotationSpeed;
    // Método chamado no primeiro frame da cena.
    private void Start()
    {
        // Seta o tamanho do array igual ao número de peixes. 
        allFish = new GameObject[numFish];
        // For loop que é executado de acordo com o número de peixes "numFish".
        for (int i = 0; i < numFish; i++)
        {
            // Pega uma posição random dentro do limite setado na variável "swinLimits".
            Vector3 pos = transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
            // Instancia o prefab (peixe), na posição random criada a cima e com a rotação zerada.
            allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
            // Seta na variável "myManager" do prefab instanciado esse script "FlockManager".
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        // Seta na variável "goalPos" a posição desse script. 
        goalPos = this.transform.position;
    }
    // Método chamado a cada frame.
    private void Update()
    {
        // Seta na variável "goalPos" a posição desse script. 
        goalPos = this.transform.position;
        // Se um valor random entre 0 e 100 for menor que 10 (10% de chance):
        if (Random.Range(0, 100) < 10)
        {
            // Seta na variável "goalPos" a posição desse script mais (+) uma posição random entre a os limites criados anteriormente na variável "swinLimits". 
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
        }
    }
}