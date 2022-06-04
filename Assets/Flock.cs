using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Vari�vel da classe FlockMaanger, que � atribuida no script "FlockManager", float para a velocidade do peixe.
    public FlockManager myManager;
    float speed;
    // M�todo chamado no primeiro frame da cena.
    private void Start()
    {
        // Atribui na vari�vel "speed", um valor random entre a velocidade m�nima e m�xima setada na classe "FlockManager".
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }
    // M�todo chamado uma vez a cada frame.
    private void Update()
    {
        // Chama o m�todo "ApplyRules()", move o peixe com o "transform.Translate" e com a velocidade setada anteriormente.
        ApplyRules();
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
            // Seta na vari�vel "vcentre" a multiplica��o entre a vari�vel "vavoid" e a vari�vel "groupSize" (tamanho do grupo).
            vcentre = vavoid * groupSize;
            // Seta na vari�vel "speed" o valor da vari�vel "gSpeed" dividido pela vari�vel "groupSize" (tamanho do grupo).
            speed = gSpeed / groupSize;
            // Vari�vel Vector3 com o valor da dire��o entre as vari�veis "vcentre" e "vavoid" com a posi��o do peixe aual.
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