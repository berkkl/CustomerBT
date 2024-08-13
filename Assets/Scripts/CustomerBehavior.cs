using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerBehavior : BTAgent
{
    public GameObject restaurantEnterence;
    public GameObject table;
    public GameObject cashier;
    private bool _hasEaten = false;

    public GameObject[] meals; // Created empty objects for prototyping
    
    private void Start()
    {
        base.Start();
        
        restaurantEnterence = GameObject.FindGameObjectsWithTag("Enterence")[0];
        table = GameObject.FindGameObjectsWithTag("Table")[0];
	    cashier = GameObject.FindGameObjectsWithTag("Cashier")[0];
 
        Sequence goToRestaurant = new Sequence("Go To Restaurant");
        Sequence eatMealSequence = new Sequence("Eat Meal Sequence");
        Sequence payForMealSequence = new Sequence("Pay For Meal Sequence");
        Leaf moveToTable = new Leaf("Move to Table", MoveToTable);
        Leaf sitToTable = new Leaf("Sit to Table", SitToTable);
        RSelector orderMeal = new RSelector("Order Meal");
        for (int i = 0; i < meals.Length; i++)
        {
			Leaf orderMealLeaf = new Leaf("Order " + meals[i].name, i, OrderMeaL);   
			orderMeal.AddChildren(orderMealLeaf);
        }
        Leaf eatMeal = new Leaf("Eat Meal", EatMeal);
        Leaf goToCashier = new Leaf("Go To Cashier", GoToCashier);
        Leaf payForMeal = new Leaf("Pay For Meal", PayForMeal);
        Leaf exitRestaurant = new Leaf("Exit Restaurant", ExitRestaurant);
        
        goToRestaurant.AddChildren(eatMealSequence);
        goToRestaurant.AddChildren(payForMealSequence);
        goToRestaurant.AddChildren(exitRestaurant);

		eatMealSequence.AddChildren(moveToTable);
		eatMealSequence.AddChildren(sitToTable);
		eatMealSequence.AddChildren(orderMeal);
		eatMealSequence.AddChildren(eatMeal);

        payForMealSequence.AddChildren(goToCashier);
        payForMealSequence.AddChildren(payForMeal);

		tree.AddChildren(goToRestaurant);
    }

	public Node.Status MoveToTable()
    {
        Node.Status s = GoToDestination(table.transform.position);
        if(s == Node.Status.SUCCESS)
        {
            Debug.Log($"{this.name} has reached to the table.");
		}
        return s;
    }

    public Node.Status SitToTable()
    {
        Debug.Log($"{this.name} is sitting to {table.name}.");
        return Node.Status.SUCCESS;
    }

    public Node.Status OrderMeaL(int i)
    {
	    switch (i)
	    {
		    case 0:
			    return OrderPotato();
		    case 1:
			    return OrderChicken();
		    default:
			    return OrderPotato();
	    }
    }

	public Node.Status OrderPotato()
	{
		Debug.Log($"{this.name} is ordering Potato.");
        if (meals[0].activeSelf) return Node.Status.SUCCESS;
		else return Node.Status.FAILURE;
	}

	public Node.Status OrderChicken()
	{
		Debug.Log($"{this.name} is ordering Chicken.");
		if (meals[1].activeSelf) return Node.Status.SUCCESS;
		else return Node.Status.FAILURE;
		
	}


	public Node.Status EatMeal()
    {
        Debug.Log($"{this.name} is eating meal.");
        return Node.Status.SUCCESS;
    }

	private Node.Status GoToCashier()
	{
		return GoToDestination(cashier.transform.position);
	}

	public Node.Status PayForMeal()
    {
		Debug.Log($"{this.name} is paying for the meal.");
        return Node.Status.SUCCESS;
	}

    public Node.Status ExitRestaurant()
    { 
        Node.Status s = GoToDestination(restaurantEnterence.transform.position, 1);
        if(s == Node.Status.SUCCESS)
        {
			_hasEaten = true;
            this.gameObject.SetActive(false);
			Debug.Log($"{this.name} has exited the restaurant.");
		}
        return s;
    }
}