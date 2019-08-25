# Smart Save for Unity3D

A simple save system for Unity3D

## Installation

Import the [last package](https://github.com/nicoplv/smart-save/releases) on your project and that's all!

## Usage
Inherit from the class SmartSaves.Data to create a save class
```C#
public class MySaveClass : SmartSaves.Data<MySaveClass>
{
  public string MyVarToSave;
  
  [SerializeField]
  private string myPrivateVarToSave;
  public string MyPrivateVarToSave { get { return myPrivateVarToSave;} }
  public void SetMyPrivateVarToSave(string _value) { myPrivateVarToSave = _value; }
}
```

At the start of your game you need to create and load your data
```C#
MySaveClass mySaveClass = MySaveClass.Create("NameOfTheSaveFile");
mySaveClass.Load();
```

You can do all the modifications you want in your object after
```C#
mySaveClass.MyVarToSave = "NewValue";
mySaveClass.SetMyPrivateVarToSave("NewValue");
```

But don't forget to save all the modifications after
```C#
mySaveClass.Save();
```

## Settings
You can change the type of save by modifying the SaveType variable in SmartSave/Resources/SmartSaveSettings.asset
- Text : will save your data in text
- Binary : will save your data in binary
- Binary And Checksum : will save your data in binary and add a check sum at the end to be sure the save file has not been edited
