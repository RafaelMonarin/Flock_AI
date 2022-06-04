using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Variável da classe FlockMaanger, que é atribuida no script "FlockManager", float para a velocidade do peixe.
    public FlockManager myManager;
    float speed;
    // Método chamado no primeiro frame da cena.
    private void Start()
    {
        // Atribui na variável "speed", um valor random entre a velocidade mínima e máxima setada na classe "FlockManager".
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }
    // Método chamado uma vez a cada frame.
    private void Update()
    {
        // Chama o método "ApplyRules()", move o peixe com o "transform.Translate" e com a velocidade setada anteriormente.
        ApplyRules();
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
            // Seta na variável "vcentre" a multiplicação entre a variável "vavoid" e a variável "groupSize" (tamanho do grupo).
            vcentre = vavoid * groupSize;
            // Seta na variável "speed" o valor da variável "gSpeed" dividido pela variável "groupSize" (tamanho do grupo).
            speed = gSpeed / groupSize;
            // Variável Vector3 com o valor da direção entre as variáveis "vcentre" e "vavoid" com a posição do peixe aual.
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