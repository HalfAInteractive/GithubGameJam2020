using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Human random will attempt to return random integers within a specified range without single numbers occuring too often.
// Human brain likes seeing a roughly equal distribution of the range.
public class HumanRandom : MonoBehaviour
{
    [SerializeField] private int min = 0;
    [SerializeField] private int max = 10;
    private int maxRepeats = 2; // initializes to 2.
    
    // If humanBrain 
    private const int humanBrain = 2; // How many numbers until the brain wants a number still unused.
    
    private int count = 0; // How many numbers are at full capacity. It might be time to allow repeats.
    private int floor = 0; // If every number has been hit, reduce all numbers.
    private int buffer = 0; // How many repeats did we get without a single new number.
    
    private Dictionary<int, int> prevValues;
    
    void Start()
    {
        
    }

    public void Reset()
    {
        count = 0;
        floor = 0;
        buffer = 0;

        SetRange(min, max);
    }
    
    public void SetRange(int A, int B)
    {
        prevValues = new Dictionary<int, int>();

        min = A;
        max = B;

        for (int i = min; i < max; i++)
        {
            prevValues.Add(i, 0);
        }
    }
    
    public int GetNextRand()
    {
        while(true)
        {
            // Try a number
            int temp = (int) Random.Range(min, max);
            // Do we have too many consecutive repeats
            if(buffer == humanBrain)
            {
                buffer = 0;
                floor++;
                if(floor == max - min)
                {
                    floor = 0;
                    LowerToFloor();
                }
                temp = GetUnused();
                prevValues[temp]++;
                return temp;
            }
            else if(CanUse(temp)) // If not, then just get any number.
            {
                prevValues[temp]++;
                if(prevValues[temp] == 1) // If this is a new number, great!
                {
                    buffer = 0;
                    floor++;
                    if(floor == max - min)
                    {
                        floor = 0;
                        LowerToFloor();
                    }
                }
                else // Otherwise its a repeat. Increment the "buffer".
                {
                    buffer++;
                    if(prevValues[temp] == maxRepeats) // This number will no longer pass the CanUse check.
                    {
                        count++;
                        if(count >= (max - min)*0.7f) // 0.7 was chosen arbitrarily. If 0.7 of the range has maxed out. They can probably come up again. 0.8, 0.9 may work better if you really want all numbers used.
                        {
                            count = 0;
                            maxRepeats++;
                        }
                    }
                }
                return temp;
            }
        }   
    }

    public int GetNextRandSingle()
    {
        if (count == max - min)
        {
            return -1;
        }
        while (true)
        {
            int temp = Random.RandomRange(min, max);
            if (Unused(temp))
            {
                count++;
                prevValues[temp]++;
                return temp;
            }
        }
    }
    
    private int GetUnused()
    {
        while(true)
        {
            int temp = Random.Range(min, max);
            if(Unused(temp))
            {
                return temp;
            }
        }
    }
    
    private bool CanUse(int rand)
    {
        return prevValues[rand] != maxRepeats;
    }
    
    private bool Unused(int rand)
    {
        return prevValues[rand] == 0;
    }
    
    private void LowerToFloor()
    {
        for(int i = min; i <= max; i++)
        {
            prevValues[i]--;
        }
    }
}
