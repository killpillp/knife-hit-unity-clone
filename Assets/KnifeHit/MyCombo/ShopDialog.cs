using UnityEngine;
using UnityEngine.UI;

public class ShopDialog : MonoBehaviour
{
    public Text[] numRubyTexts, priceTexts;

    protected void Start()
    {
#if IAP && UNITY_PURCHASING
        Purchaser.instance.onItemPurchased += OnItemPurchased;

        for(int i = 0; i < numRubyTexts.Length; i++)
        {
            numRubyTexts[i].text = Purchaser.instance.iapItems[i].value + " rubies";
            priceTexts[i].text = Purchaser.instance.iapItems[i].price + "$";
        }
#endif
    }

    public void OnBuyProduct(int index)
	{
#if IAP && UNITY_PURCHASING
        Sound.instance.PlayButton();
        Purchaser.instance.BuyProduct(index);
#else
        Debug.LogError("Please enable, import and install Unity IAP to use this function");
#endif
    }

#if IAP && UNITY_PURCHASING
    private void OnItemPurchased(IAPItem item, int index)
    {
        // A consumable product has been purchased by this user.
        if (item.productType == PType.Consumable)
        {
            CurrencyController.CreditBalance(item.value);
            Toast.instance.ShowMessage("Your purchase is successful");
            CUtils.SetBuyItem();
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (item.productType == PType.NonConsumable)
        {
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        // Or ... a subscription product has been purchased by this user.
        else if (item.productType == PType.Subscription)
        {
            // TODO: The subscription item has been successfully purchased, grant this to the player.
        }
    }
#endif

#if IAP && UNITY_PURCHASING
    private void OnDestroy()
    {
        Purchaser.instance.onItemPurchased -= OnItemPurchased;
    }
#endif
}
