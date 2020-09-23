using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleJogador : MonoBehaviour
{
    public Rigidbody jogador;
    public GameObject cenario;
    public float velocidadeCenario;

    public float distanciaRaia;
    private int raiaAtual;

    private Vector3 target;

    private Vector2 initialPosition;

    public GameObject chao;
    public GameObject cube;
    public GameObject moeda;
    // public text txtGameOver;
    // public Text txtPontos;

    private int estagioAtual = 1;

    private int pontos = 0;

    void Start()
    {
        raiaAtual = 1;
        target = jogador.transform.position;
        montarCenario();
        // txtPontos.text = "" + pontos;
    }



    void Update()

    {
        transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);

        // x-> lateral, y-> altura, z->profundida
        int novaRaia = -1;

        // teclado
        if (Input.GetKeyDown(KeyCode.RightArrow) && raiaAtual < 2)
        {
            novaRaia = raiaAtual + 1;

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && raiaAtual > 0)
        {
            novaRaia = raiaAtual - 1;
        }

        //mouse
        if (Input.GetMouseButtonDown(0))
        {
            initialPosition = Input.mousePosition;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Input.mousePosition.x > initialPosition.x && raiaAtual < 2)
            {
                novaRaia = raiaAtual + 1;

            }
            else if (Input.mousePosition.x < initialPosition.x && raiaAtual > 0)
            {
                novaRaia = raiaAtual - 1;
            }
        }
        // touch
        if (Input.touchCount >= 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                initialPosition = Input.GetTouch(0).position;

            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                if (Input.GetTouch(0).position.x > initialPosition.x && raiaAtual < 2)
                {
                    novaRaia = raiaAtual + 1;
                }
                else if (Input.GetTouch(0).position.x < initialPosition.x && raiaAtual > 0)
                {
                    novaRaia = raiaAtual - 1;
                }
                {

                }
            }
        }

        if (novaRaia >= 0)
        {
            raiaAtual = novaRaia;
            target = new Vector3((raiaAtual - 1) * distanciaRaia, jogador.transform.position.y, jogador.transform.position.z);
            //jogador.transform.position = pos; 
        }
        if (jogador.transform.position.x != target.x)
        {
            jogador.transform.position = Vector3.Lerp(jogador.transform.position, target, 5 * Time.deltaTime);
        }


        //mover o chão 
        cenario.transform.Translate(0, 0, velocidadeCenario * Time.deltaTime * -1);

        float estagio = Mathf.Floor(((cenario.transform.position.z - 50.0F) / -100.0F)) + 1;
        if (estagio > estagioAtual)
        {
            GameObject chao2 = Instantiate(chao);
            float chao2z = ((100 * estagioAtual) + 50) + cenario.transform.position.z;
            chao2.transform.SetParent(cenario.transform);
            chao2.transform.position = new Vector3(chao.transform.position.x, chao.transform.position.y, chao2z);
            estagioAtual++;
            montarCenario();
        }

    }

    void montarCenario()
    {
        for (int i = 2; i < 10; i++)
        {
            int elemento1 = Random.Range(0, 3);
            int elemento2 = Random.Range(0, 3);
            int elemento3 = Random.Range(0, 3);
            //0 = nada / 1 = cubo / 2=moeda
            if (elemento1 == 1 && elemento2 == 1 && elemento3 == 1)
            {
                elemento2 = 0;
            }
            if (elemento1 == 1)
            {
                instanciarCubo(i, 0);
            }
            else if (elemento1 == 2)
            {
                instanciarMoeda(i, 0);
            }
            if (elemento2 == 1)
            {
                instanciarCubo(i, 1);
            }
            else if (elemento2 == 2)
            {
                instanciarMoeda(i, 1);
            }
            if (elemento3 == 1)
            {
                instanciarCubo(i, 2);
            }
            else if (elemento3 == 2)
            {
                instanciarMoeda(i, 2);
            }
        }
    }

    void instanciarCubo(int posicaoz, int posicaox)
    {
        GameObject cubo2 = Instantiate(cube);
        float posz = ((10 * posicaoz) + ((estagioAtual - 1) * 100)) +
        cenario.transform.position.z;
        float posx = (posicaox - 1) * distanciaRaia;
        cubo2.transform.SetParent(cenario.transform);
        cubo2.transform.position = new Vector3(posx, 2F, posz);
    }

    void instanciarMoeda(int posicaoz, int posicaox)
    {
        GameObject moeda2 = Instantiate(moeda);
        float posz = ((10 * posicaoz) + ((estagioAtual - 1) * 100)) +
        cenario.transform.position.z;
        float posx = (posicaox - 1) * distanciaRaia;
        moeda2.transform.SetParent(cenario.transform);
        moeda2.transform.position = new Vector3(posx, 0.5F, posz);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Moeda"))
        {
            Destroy(col.gameObject);
        }
        if (col.gameObject.CompareTag("Obstaculo"))
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

}
/*
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Moeda"))
        {
            Destroy(col.gameObject);
            pontos++;
            txtPontos.text = "" + pontos;

        }
        if (col.gameObject.CompareTag("Obstaculo"))
        {
            txtGameOver.text = "GAME OVER";
            isGameOver = true;
            invoke("GameOver", 5); 
        }
    }
}
*/ 

  