[![build](https://github.com/alexandrov-ua/PolymorphicTypePropertyGenerator/actions/workflows/build.yml/badge.svg)](https://github.com/alexandrov-ua/PolymorphicTypePropertyGenerator/actions/workflows/build.yml/badge.svg)
## Description:

Library for generating Type property and enum for polymorphic types.

## Problem:

Often when you have polymorphic contracts(DTOs), for example:
```C#
public class Response
{
  public Payment[] Payments { get; set; }
}

public class Payment
{
  public string Id {get; set;}
}

public class Cash : Payment
{
  public string OperatorId { get; set; }
}

public class Card : Payment
{
  public string CardNumber { get; set; }
}
```
In order to deserialize it on the client side, you should add a Type property and enum that describes type structure, like so: 
```C#
public class Payment
{
  public string Id {get; set;}
  public PaymentType Type { get; set; }
}

public enum PaymentType
{
  Cash, Card
}

public class Cash : Payment
{
  public Cash()
  {
    Type = PaymentType.Cash;
  }
  public string OperatorId { get; set; }
}

public class Card : Payment
{
  public Card()
  {
    Type = PaymentType.Card;
  }
  public string CardNumber { get; set; }
}
```
Its seems like unnecessary work. Especially annoying is to maintain this enum.

## Solution:

Using this lib gives you ability to write code, like so:
```C#
[PolymorphicTypeProperty]
public partial class Payment
{
  public string Id {get; set;}
}

public partial class Cash : Payment
{
  public string OperatorId { get; set; }
}

public partial class Card : Payment
{
  public string CardNumber { get; set; }
}
```
And that's it.
