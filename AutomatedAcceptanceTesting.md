Code name: Accipio

### 1. Specifying business actions ###
  * Who: business analysts
  * When: typically at the start of the project, once the requirements are known. Also if the requirements change.
  * Inputs: functional specifications, GUI mock-ups
  * Outputs: XML document describing available business actions, something like:
```
<actions>
    <action id="GoToPortal">
       <description>Open the online banking portal web site in the browser</description>
    </action>

    <action id="SignIn">
       <description>Sign in user '{0}'</description>
       <parameter name="username"/>
       <parameter name="password"/>
    </action>

    <action id="ViewAccount">
       <description>Click on the "View" button for the account '{0}'</description>
       <parameter name="accountId" type="int"/>
    </action>

    <action id="ClickActionTranfer">
       <description>Click on the "Transfer" button in the account actions list</description>
    </action>

    <action id="EnterTransferAmount">
       <description>Enter the amount of {0} to transfer</description>
       <parameter name="transferAmount" type="decimal"/>
    </action>

    <action id="EnterDestinationAccountNumber">
       <description>Enter the destination account number '{0}'</description>
       <parameter name="destAccountId" type="int"/>
    </action>

    <action id="ConfirmTransfer">
       <description>Click on the "OK" button to confirm the transfer of money between accounts</description>
    </action>

    <action id="AssertOperationSuccessful">
       <description>Assert the operation was successful</description>
    </action>

    <function id="TransferMoney">
        <steps>
            <run action="ViewAccount"/>
            <run action="ClickActionTranfer"/>
            <run action="EnterTransferAmount"/>
            <run action="EnterDestinationAccountNumber"/>
            <run action="ConfirmTransfer"/>
        <steps>        
    </function>
</actions>
```

NOTE: a special XML schema (XSD) will be available for this type of XML file. This will make writing such files easier in VisualStudio, since VS provides Intellisense for XML files with schemas. It will also enable automatic validation of actions files.

### 2. Generating test specifications XML schema file ###
  * Who: automatic tool
  * When: every time business actions XML file changes
  * Inputs: business actions XML file
  * Outputs: XML schema file for writing XML test specifications using the available business actions

### 3. Writing test cases ###
  * Who: business analysts, testers, developers
  * When: throughout the project lifetime
  * Inputs: business actions XML file, XML schema (see step 2)
  * Outputs: XML files containing test cases, like:
```
<suite id="Banking" runner="OnlineBanking">
    <description>Contains test cases which cover online banking functionality.</description>
    <case id="MoneyTransfer">
        <description>Tests money transfers from one account to another using the online banking portal.</description>
        <tags>R15</tags>
        <tags>R211</tags>
        <steps>
            <GoToPortal/>
            <SignIn username="john" password="doe"/>
            <ViewAccount accountId="34528"/>
            <ClickActionTranfer/>
            <EnterTransferAmount>644.33</EnterTransferAmount>
            <EnterDestinationAccountNumber>23677</EnterDestinationAccountNumber>
            <ConfirmTransfer/>
            <AssertOperationSuccessful/>
        </steps>
    </case>
<suite>
```

### 4. Generating testing source code and documentation ###
  * Who: automatic tool
  * When: in each build
  * Inputs: business actions XML file, business actions XML schema, test cases XML file
  * Outputs:
    * C# code files which execute the specified tests
    * Test case documentation in HTML format

The sample generated source code:
```
/// <summary>Contains test cases which cover online banking functionality.</summary>
public class BankingTestSuite
{
   /// <summary>Tests money transfers from one account to another using the online banking portal.</summary>
   public void MoneyTransferTestCase
   {
      using (OnlineBankingTestRunner runner = new OnlineBankingTestRunner())
      {
         runner
            .AddDescription("Tests money transfers from one account to another using the online banking portal.")
            .AddTag ("R15")
            .AddTag ("R211")

            // Open the online banking portal web site in the browser
            .GoToPortal()
            // ...
            .SignIn("john", "doe")
            // Click on the "View" button for the account '34528'
            .ViewAccount(34528)
            // ...
            .ClickActionTranfer()
            // ...
            .EnterTransferAmount(644.33)
            // ...
            .EnterDestinationAccountNumber(23677)
            // ...
            .ConfirmTransfer()
            // ...
            .AssertOperationSuccessful();
      }
   }
}
```

### 5. Implementing testing code ###
  * Who: developers, testers
  * When: from the project start until all of the test actions have been implemented
  * Inputs: business actions XML file
  * Outputs: C# TestRunner(s) code

The test runner is a C# class which is executed in generated tests. Each test action specified in the step 1 must be implemented as a method of the test runner. It will have a common base (skeleton) implementation (called TestRunnerBase) available in the library from which all test runners should inherit.

### 6. Running tests ###
  * Who: anyone + automated tool
  * When: throughout the project lifecycle
  * Inputs: C# test code
  * Outputs: test results in XML form

The test runner should emit an XML log file with the results of testing. These results will then be automatically fed to some reporting tool (CruiseControl.NET or something else) to visualize test results.

Each test case can have three different outcomes:
  * Successful
  * Failed
  * Not yet implemented - if any of the test actions throw NotImplementedException

In addition to showing results per test case, they should also be grouped by specifications they cover.