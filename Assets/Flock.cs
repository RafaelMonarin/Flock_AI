using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Variável da classe FlockMaanger, que é atribuida no script "FlockManager", float para a velocidade do peixe, bool para verificar se o peixe está rodando.
    public FlockManager myManager;
    float speed;
    bool turning = false;
    // Método chamado no primeiro frame da cena.
    private void Start()
    {
        // Atribui na variável "speed", um valor random entre a velocidade mínima e máxima setada na classe "FlockManager".
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }
    // Método chamado uma vez a cada frame.
    private void Update()
    {
        // Declaração do Bounds passando a ela o centro e o tamanho.
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        // Declaração do Raycast.
        RaycastHit hit = new RaycastHit();
        // Declaração de um vetor direção entre as posições do myManager (classe "FlockManager") e esse script (esse peixe).
        Vector3 direction = myManager.transform.position - transform.position;
        // Se o esse peixe não estiver dentro do Bounds:
        if (!b.Contains(transform.position))
        {
            // Seta na variável "turning" para verdadeiro.
            turning = true;
            // Seta na variável "direction" um vetor direção entre as posições do myManager (classe "FlockManager") e esse script (esse peixe).
            direction = myManager.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            // Seta a variável "turning" para verdadeiro.
            turning = true;
            // Seta na variável "direction" um vetor direção de reflexão da direção da frente do peixe com a normal da colisão (quando o peixe colidir ele reflete no objeto).
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            // Seta a variável "turning" para falso.
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
                // Seta na variável "speed" um valor random entre a velocidade mínima e máxima da classe "FlockManager".
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }
            // Se um valor random entre 0 e 100 for menor que 20 (20% de chance):
            if (Random.Range(0, 100) < 20)
            {
                // Chama o método "ApplyRules()";
                ApplyRules();
            }
        }
        // Move o peixe com transform.Transate.
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    // Método onde está as regras para o cardume.
    void ApplyRules()
    {
        // Array de GameObjects com o tamanho igual ao número de peixes setado na classe "FlockManager".
        GameObject[] gos;
        gos = myManager.allFish;
        // Variáveis Vector3, float e int para as regras do cardume.
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
                // Seta na variável "nDistance" o valor da distância entre um peixe e o peixe atual.
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                // Se a distância setada anteriormente for menor ou igual a distância entre os vizinhos: 
                if (nDistance <= myManager.neighbourDistance)
                {
                    // Adiciona na variável "vcentre" a posição do peixe.
                    vcentre += go.transform.position;
                    // + 1 para a variável "groupSize" (aumenta o tamanho do grupo).
                    groupSize++;
                    // Se a distância for menor que 1:
                    if (nDistance < 1)
                    {
                        // Seta na variável "vavoid" o valor dela mesma mais (+) (a posição do peixe atual menos (-) a posição de um peixe).
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    // Variável da classe "Flock" que é atribuida com a classe "Flock" co peixe.
                    Flock anotherFlock = go.GetComponent<Flock>();
                    // Seta na variável "gSpeed" ela mesma mais (+) a velocidade da classe "Flock".
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        // Se a variável "groupSize" (tamanho do grupo) for maior que 0:
        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;
            Vector3 direction = (vcentre + vavoid) - transform.position;
            // Se a variável "direction" for diferente de zero:
            if (direction != Vector3.zero)
            {
                // Seta a rotação do peixe com o Quaternion.Slerp.
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}