using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    // Vari�vel GameObject para o prefab do peixe, int para o n�mero de peixes, array de GameObjects para todos os peixes, e Vector3 (�rea para instanciar os peixes).
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;
    // Vari�veis float para o cardume.
    [Header("Configura��es do Cardume")]
    [Range(0f, 5f)]
    public float minSpeed;
    [Range(0f, 5f)]
    public float maxSpeed;
    [Range(1f, 10f)]
    public float neighbourDistance;
    [Range(0f, 5f)]
    public float rotationSpeed;
    // M�todo chamado no primeiro frame da cena.
    private void Start()
    {
        // Seta o tamanho do array igual ao n�mero de peixes. 
        allFish = new GameObject[numFish];
        // For loop que � executado de acordo com o n�mero de peixes "numFish".
        for (int i = 0; i < numFish; i++)
        {
            // Pega uma posi��o random dentro do limite setado na vari�vel "swinLimits".
            Vector3 pos = transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
            // Instancia o prefab (peixe), na posi��o random criada a cima e com a rota��o zerada.
            allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
            // Seta na vari�vel "myManager" do prefab instanciado esse script "FlockManager".
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        // Seta na vari�vel "goalPos" a posi��o desse script. 
        goalPos = this.transform.position;
    }
    // M�todo chamado a cada frame.
    private void Update()
    {
        // Seta na vari�vel "goalPos" a posi��o desse script. 
        goalPos = this.transform.position;
        // Se um valor random entre 0 e 100 for menor que 10 (10% de chance):
        if (Random.Range(0, 100) < 10)
        {
            // Seta na vari�vel "goalPos" a posi��o desse script mais (+) uma posi��o random entre a os limites criados anteriormente na vari�vel "swinLimits". 
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
        }
    }
}