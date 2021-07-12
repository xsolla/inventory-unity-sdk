# Cross-Buy asset for Unity

Game Commerce asset is a set of classes and methods that you can integrate into your Unity app to work with Xsolla products.

After integration of the asset, you can use:

*   [Login](https://developers.xsolla.com/doc/login/) to authenticate users and manage the friend system and user account
*   [In-Game Store](https://developers.xsolla.com/doc/in-game-store/) to manage player’s inventory

The asset is suitable if your application is published on a platform that cannot use Xsolla solutions for payments and in-game store for some reason.

[Try our demo to learn more](https://livedemo.xsolla.com/sdk/unity/webgl/).

![Cross Buy asset](https://i.imgur.com/sschstq.png "Cross Buy asset")


<div style="background-color: WhiteSmoke">
<p><b>Note:</b></p>
<p>
Cross-Buy asset is already included as part of the <a href="https://assetstore.unity.com/packages/tools/integration/xsolla-game-commerce-145141">Game Commerce</a> asset. You can download the Game Commerce if you need a broader set of features.
</p>
<p>
Cross-Buy asset includes <a href="https://assetstore.unity.com/packages/tools/integration/xsolla-login-account-system-180654">Login & Account System</a> asset.
</p>
<p>Do <b>NOT</b> install these assets separately.</p>
</div>

For a better understanding of which Xsolla asset to choose, use the table:
<table>
  <tr>
   <td>
   </td>
   <td style="text-align: center"><b>Game Commerce asset</b>
   </td>
   <td style="text-align: center"><b>Login & Account System asset</b>
   </td>
   <td style="text-align: center"><b>Cross-Buy asset</b>
   </td>
  </tr>
  <tr>
   <td colspan="4" ><b>In-game store</sb>
   </td>
  </tr>
  <tr>
   <td>
    Virtual currency
   </td>
   <td>+
   </td>
   <td>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>
    Virtual items
   </td>
   <td>+
   </td>
   <td>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>
    Player inventory
   </td>
   <td>+
   </td>
   <td>
   </td>
   <td>+
   </td>
  </tr>
  <tr>
   <td>
    Bundles
   </td>
   <td>+
   </td>
   <td>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>
    Promotional campaigns
   </td>
   <td>+
   </td>
   <td>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="4" ><b>Login</b>
   </td>
  </tr>
  <tr>
   <td>
    Authentication
   </td>
   <td>+
   </td>
   <td>+
   </td>
   <td>+
   </td>
  </tr>
  <tr>
   <td>
    User management
   </td>
   <td>+
   </td>
   <td>+
   </td>
   <td>+
   </td>
  </tr>
  <tr>
   <td><strong>Payment UI</strong>
   </td>
   <td>+
   </td>
   <td>
   </td>
   <td>
   </td>
  </tr>
  <tr>
  <td colspan="4" ><b>Additional features</b>
   </td>
  </tr>
  <tr>
   <td>
    UI builder
   </td>
   <td>
    +
   </td>
   <td>
    +
   </td>
   <td>
    +
   </td>
  </tr>
  <tr>
   <td>
    Battle pass
   </td>
   <td>
    +
   </td>
   <td>
   </td>
   <td>
   </td>
  </tr>
</table>

## Requirements

### System requirements

*   64-bit OS
*   Windows 7 SP1 and later
*   macOS 10.12 and later
*   The version of Unity not earlier than 2019.4.19f1

### Target OS

*   Android
*   macOS
*   Windows 64-bit

Additionally, the asset supports [creating WebGL build](https://developers.xsolla.com/sdk/unity/how-tos/application-build/#unity_sdk_how_to_build_webgl) to run your application in a browser.

<div style="background-color: WhiteSmoke">
<p><b>Note:</b></p>
<p>We recommend you use the Mono compiler for desktop platforms as it's compatible with the provided in-game browser. If you use other browser solutions, you can use the IL2CPP compiler instead. To create game builds for Android, you can use either Mono or IL2CPP compilers.</p>
</div>

## Integration

Before you integrate the asset, you need to sign up to [Publisher Account](https://publisher.xsolla.com/signup?store_type=sdk) and set up a new project.

More instructions are on the [Xsolla Developers portal](https://developers.xsolla.com/sdk/unity/cross-buy/).


## Usage 

Xsolla provides APIs to work with it’s products. The Game Commerce asset provides classes and methods for API calls, so you won’t need to write boilerplate code. Use the [tutorials](https://developers.xsolla.com/sdk/unity/tutorials/) to learn how you can use the [asset methods](https://developers.xsolla.com/sdk-code-references/unity-store/).


## Legal info

[Explore legal information](https://developers.xsolla.com/sdk/unity/cross-buy/get-started/#sdk_legal_compliance) that helps you work with Xsolla.

Xsolla offers the necessary tools to help you build and grow your gaming business, including personalized support at every stage. The terms of payment are determined by the contract that you can sign in Xsolla Publisher Account.

---

### License

See the [LICENSE](https://github.com/xsolla/inventory-unity-sdk/blob/master/LICENSE.txt) file.


### Community

[Join our Discord server](https://discord.gg/auNFyzZx96) and connect with the Xsolla team and developers who use Xsolla products.


### Additional resources

*   [Xsolla official website](https://xsolla.com/)
*   [Developers documentation](https://developers.xsolla.com/sdk/unity/)
*   [Code reference](https://developers.xsolla.com/sdk-code-references/unity-store/)
*   API reference:
    *   [Login API](https://developers.xsolla.com/login-api/) 
    *   [Commerce API](https://developers.xsolla.com/commerce-api/player-inventory)