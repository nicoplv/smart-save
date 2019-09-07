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

You do all the modifications you want in your object
```C#
mySaveClass.MyVarToSave = "NewValue";
mySaveClass.SetMyPrivateVarToSave("NewValue");
```

And don't forget to save all the modifications at the end
```C#
mySaveClass.Save();
```

## Settings
You can change the type of save by modifying SmartSave/Resources/SmartSaveSettings.asset:
- Create a new config with the +
- Select the platform who use this config
- Choose the type system (currently the only one proposed is Persistent data path file, but you can write your own one)
- Set settings of the system like if if the file is write in binary, protected with a checksum, ...

## Advanced Usage
When you create a save with a different config than the one on your settings, you just have to add a configuration when you create it
```C#
MySaveClass mySaveClass = MySaveClass.Create("NameOfTheSaveFile", SmartSaves.SaveSystems.Config.ForPersistentDataPathFile<MySaveClass>(binary: true, checksum: true, shuffle: SmartSaves.SaveSystems.Config.PersistentDataPathFileShuffleTypes.Random));
mySaveClass.Load();
```