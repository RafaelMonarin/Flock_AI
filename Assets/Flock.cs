using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Vari�vel da classe FlockMaanger, que � atribuida no script "FlockManager", float para a velocidade do peixe, bool para verificar se o peixe est� rodando.
    public FlockManager myManager;
    float speed;
    bool turning = false;
    // M�todo chamado no primeiro frame da cena.
    private void Start()
    {
        // Atribui na vari�vel "speed", um valor random entre a velocidade m�nima e m�xima setada na classe "FlockManager".
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }
    // M�todo chamado uma vez a cada frame.
    private void Update()
    {
        // Declara��o do Bounds passando a ela o centro e o tamanho.
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        // Declara��o do Raycast.
        RaycastHit hit = new RaycastHit();
        // Declara��o de um vetor dire��o entre as posi��es do myManager (classe "FlockManager") e esse script (esse peixe).
        Vector3 direction = myManager.transform.position - transform.position;
        // Se o esse peixe n�o estiver dentro do Bounds:
        if (!b.Contains(transform.position))
        {
            // Seta na vari�vel "turning" para verdadeiro.
            turning = true;
            // Seta na vari�vel "direction" um vetor dire��o entre as posi��es do myManager (classe "FlockManager") e esse script (esse peixe).
            direction = myManager.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            // Seta a vari�vel "turning" para verdadeiro.
            turning = true;
            // Seta na vari�vel "direction" um vetor dire��o de reflex�o da dire��o da frente do peixe com a normal da colis�o (quando o peixe colidir ele reflete no objeto).
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            // Seta a vari�vel "turning" para falso.
            turning = false;
        }
        // Se "turning" for verdadeiro:
        if (turning)
        {
            // Roda o peixe com Quaternion.Slerp.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Se um valor random entre 0 e 100 for menor que 10 (10% de chance):
            if (Random.Range(0, 100) < 10)
            {
                // Seta na vari�vel "speed" um valor random entre a velocidade m�nima e m�xima da classe "FlockManager".
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }
            // Se um valor random entre 0 e 100 for menor que 20 (20% de chance):
            if (Random.Range(0, 100) < 20)
            {
                // Chama o m�todo "ApplyRules()";
                ApplyRules();
            }
        }
        // Move o peixe com transform.Transate.
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    // M�todo onde est� as regras para o cardume.
    void ApplyRules()
    {
        // Array de GameObjects com o tamanho igual ao n�mero de peixes setado na classe "FlockManager".
        GameObject[] gos;
        gos = myManager.allFish;
        // Vari�veis Vector3, float e int para as regras do cardume.
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;
        // Loop para arrays, que pega 1 GameObject por vez dentro do array de GameObjects setado anteriormente.
        foreach (GameObject go in gos)
        {
            // Se o GameObject (peixe) for diferente a esse GameObject (peixe atual): 
            if (go != this.gameObject)
            {
                // Seta na vari�vel "nDistance" o valor da dist�ncia entre um peixe e o peixe atual.
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                // Se a dist�ncia setada anteriormente for menor ou igual a dist�ncia entre os vizinhos: 
                if (nDistance <= myManager.neighbourDistance)
                {
                    // Adiciona na vari�vel "vcentre" a posi��o do peixe.
                    vcentre += go.transform.position;
                    // + 1 para a vari�vel "groupSize" (aumenta o tamanho do grupo).
                    groupSize++;
                    // Se a dist�ncia for menor que 1:
                    if (nDistance < 1)
                    {
                        // Seta na vari�vel "vavoid" o valor dela mesma mais (+) (a posi��o do peixe atual menos (-) a posi��o de um peixe).
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    // Vari�vel da classe "Flock" que � atribuida com a classe "Flock" co peixe.
                    Flock anotherFlock = go.GetComponent<Flock>();
                    // Seta na vari�vel "gSpeed" ela mesma mais (+) a velocidade da classe "Flock".
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        // Se a vari�vel "groupSize" (tamanho do grupo) for maior que 0:
        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;
            Vector3 direction = (vcentre + vavoid) - transform.position;
            // Se a vari�vel "direction" for diferente de zero:
            if (direction != Vector3.zero)
            {
                // Seta a rota��o do peixe com o Quaternion.Slerp.
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}