using UnityEngine;

public class ObjetVivant : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask layerSol;
    public LayerMask layerVivant;
    
    public VivantConf vivantConf;
    public MeshRenderer renderer;

    public Rigidbody rb;

    private Vector3 _target;
    private float _targetTimer;
    private float _jumpTimer;
    private Transform _foodTarget;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _target = transform.position;
        float random = Random.value;
        float randomSize = Mathf.Lerp(vivantConf.tailleRandom.x, vivantConf.tailleRandom.y, random);
        transform.localScale = Vector3.one * randomSize;
        rb.mass = Mathf.Lerp(vivantConf.masseRandom.x, vivantConf.masseRandom.y, random);
        renderer.sharedMaterial = vivantConf.materiauxRandom[Random.Range(0, vivantConf.materiauxRandom.Count)];

        _jumpTimer = GetJumpInterval();
    }

    // Update is called once per frame
    void Update()
    {
        if (HandleFood())
        {
            return;
        }

        _targetTimer -= Time.deltaTime;
        if (_targetTimer <= 0f)
        {
            _targetTimer = Random.Range(vivantConf.tempsAttente.x, vivantConf.tempsAttente.y);
            if (tryPickTarget(out var t))
            {
                _target = t;
            }
        }
        
        _jumpTimer -= Time.fixedDeltaTime;
        if (_jumpTimer <= 0f && IsGrounded())
        {
            _jumpTimer = GetJumpInterval();
            float jumpForce = GetJumpForce();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool tryPickTarget(out Vector3 t)
    {
        for (int i = 0; i < 20; i++)
        {
            float mouvementX = Random.Range(vivantConf.rayonMovement.x, vivantConf.rayonMovement.y) *
                               (Random.value < 0.5f ? -1f : 1f);
            float mouvementZ = Random.Range(vivantConf.rayonMovement.x, vivantConf.rayonMovement.y) *
                               (Random.value < 0.5f ? -1f : 1f);
            Vector3 p = transform.position + new Vector3(mouvementX, 2f, mouvementZ);
            if (!Physics.Raycast(p, Vector3.down, out var hit, 10f, layerSol))
            {
                continue;
            }

            if (Physics.SphereCast(p,0.5f,Vector3.down,out var hit2,10f,layerVivant))
            {
                continue;
            }
            t = new Vector3(hit.point.x,transform.position.y,hit.point.z);
            return true;
        }
        t=Vector3.zero;
        return false;
    }

    private void FixedUpdate()
    {
        var to = (_target - rb.position);
        to.y = 0f;
        float stopDistance = _foodTarget != null ? vivantConf.distanceNourriture : vivantConf.stopMouvement;
        if (to.magnitude <= stopDistance)
        {
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            rb.AddForce(to.normalized * vivantConf.acceleration,ForceMode.Acceleration);
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, vivantConf.vitesseMax);
        }

        
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(rb.position, Vector3.down, 0.6f, layerSol);
    }

    private float GetJumpInterval()
    {
        if (vivantConf.tempsSautRandom != Vector2.zero)
        {
            return Random.Range(vivantConf.tempsSautRandom.x, vivantConf.tempsSautRandom.y);
        }
        return vivantConf.tempsSaut;
    }

    private float GetJumpForce()
    {
        if (vivantConf.forceSautRandom != Vector2.zero)
        {
            return Random.Range(vivantConf.forceSautRandom.x, vivantConf.forceSautRandom.y);
        }
        return vivantConf.vitesseSaut;
    }

    private bool HandleFood()
    {
        if (_foodTarget ==null && TryFindFood(out var food))
        {
            _foodTarget = food;
        }

        if (_foodTarget ==null)
        {
            return false;
        }

        _target = _foodTarget.position;
        float foodDistance = Vector3.Distance(transform.position, _target);
        if (foodDistance <= vivantConf.distanceNourriture)
        {
            print("mangé");
            Destroy(_foodTarget.parent.gameObject);
            _foodTarget = null;
            return false;
        }

        return true;
    }

    bool TryFindFood(out Transform food)
    {
        foreach (var col in Physics.OverlapSphere(transform.position, vivantConf.rayonNourriture))
        {
            if(!col.CompareTag("Nourriture"))
                continue;
            food = col.transform;
            return true;
        }
        food = null;
        return false;
    }
}
