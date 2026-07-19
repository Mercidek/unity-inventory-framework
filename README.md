# Unity Inventory System Framework

This tool was developed for easily creating and using inventory (logic and UI) system in any 2D/3D, Desktop/Mobile Unity project. It contains the core inventory architecture and its UI components.

## 🔑 Key Features
* Storing data via ScriptableObjects efficiently
* Stackable items system
* Event-driven communication between code and UI
* Configuring a starting item list to initialize the inventory with predefined items
* Executing chained item effects one by one via ScriptableObjects upon item consumption
* Modifying character stats dynamically via applied item effects
* Saving and loading operations using JSON-based inventory data serialization
* Responsive UI to support desktop and mobile devices
* Organized project folder structure for better layout

> **Performance Note:** Focused on minimizing the GC overhead as much as possible.

## 💻 Developer Interactions
* Creating items with various attributes, item databases for save and loading, item effects that affect the given stats all via ScriptableObjects
* Triggering dedicated C# events during item addition, removal, or inventory refreshes for accessing inventory states from outside
* Customizing the inventory UI design easily without breaking the system (logic and visual presentation are separated)

## 🔨 How to Use

### 💾 Installation

**1.** Clone this project to your local machine using Git or download it directly as a ZIP file 
```bash
   git clone https://github.com/Mercidek/unity-inventory-framework
```

**2.** Open the project using Unity Hub *(tested in version 6000.3.18f1)*

**3.** Open the **InventoryDemoScene** scene and run it to use the system

### 🔧 Getting Started
---
A demo scene is included to help developers easily get started with the system. That scene includes the example of an **InventoryHolder** GameObject which has an ability to creation and usage of the inventory.
> As given example, the **InventoryHolder** script can be attached to a desired GameObject requiring an inventory system.

In this script, various inventory features like item database, save file name, UI canvas to be shown and inventory size can be easily set from the inspector as well as a starting item list.

![InventoryHolder script](https://github.com/user-attachments/assets/0d5a3ddd-9728-483f-8397-0e6b223e51f1)

>**sampleInventoryCanvas** is a prefab that contains core predefined UI components to present inventory to the screen properly. Can be customized if intended. There is also a slot prefab for slot graphics which is also customizable.

![sampleInventoryCanvas prefab](https://github.com/user-attachments/assets/a013cf02-3dd6-4403-8106-b08631a5c255) ![slotPrefab prefab](https://github.com/user-attachments/assets/21ddb675-14d2-4d6d-8de8-30715a619365)


### 🧤 Items and Effects
---
**Creating an Item**

Items can be created from the context menu by following `Create > Inventory System > Item > New Item`

![Item ScriptableObject](https://github.com/user-attachments/assets/8a676e17-1316-4897-92f7-2999149a046c)



>Item attributes can be set from here such as ID, name, icon, max stack size and description.

**Creating an Effect**

Item effects can be created from the context menu by following `Create > Inventory System > Item > Effect > New Effect`

![Effect ScriptableObject](https://github.com/user-attachments/assets/29775f2f-f43f-43d0-913b-75fcb9dc4138)

> Stat type can be set from here which will be affected by this effect. Change amount represents how much this effect will change the desired stat (enter negative values to decrease).

**Applying Effects to Items**

![Item ScriptableObject with Effects](https://github.com/user-attachments/assets/ecceb224-f183-4d5e-8f89-522a23c9c898)


> Multiple effects can be applied to an item and arranged with a desired order.

**Consuming Items**

![StatsManager Script](https://github.com/user-attachments/assets/b40ad751-609c-4184-b19b-cf79c645c0bb)


> A **StatsManager** script attachment is required to the holder GameObject for effects to work. Stat types and current/max values of them can be set from here.

### 📄 Item Databases
---

> **An item database is required for seamless saving and loading processes.**

**Creating an Item Database**

Item databases can be created from the context menu by following `Create > Inventory System > Item > Database > New Database`

![Item Database ScriptabelObject](https://github.com/user-attachments/assets/72074809-c015-4aea-b227-2db147306c71)


> Here, previously created items can be added to the database.

### 👆 Interacting with the Inventory
---
![Inventory UI](https://github.com/user-attachments/assets/ba58f7c6-f50d-432e-ad13-2f9392b8b44b)

* Hover over an item to display its item name and description via tooltip box (can be customized within **sampleInventoryCanvas** prefab)
* Click/touch an item for consumption
* Scroll through items that won't fit in the inventory container

## ❗ Known Limitations
* Multiple inventory window layout is not yet implemented
