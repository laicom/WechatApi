<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="6aded672-8fe8-44ba-8422-0e25a07f4071" namespace="Nlab.WeChatApi.Config" xmlSchemaNamespace="urn:WeChatApi.Config" assemblyName="Nlab.WeChatApi" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="WeChatConfigSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="WeChat">
      <elementProperties>
        <elementProperty name="ReplyModules" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="replyModules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/ReplyModulesCollection" />
          </type>
        </elementProperty>
        <elementProperty name="ReplySettings" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="replySettings" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/ReplySettingCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Templates" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="templates" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/TemplateCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="ReplyModulesCollection" xmlItemName="add" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/ReplyModulesCollectionElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ReplyModulesCollectionElement">
      <attributeProperties>
        <attributeProperty name="ModuleName" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="ReplySettingCollection" xmlItemName="add" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/ReplySettingCollectionElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ReplySettingCollectionElement">
      <attributeProperties>
        <attributeProperty name="ModuleName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="moduleName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="MatchText" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="matches" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="MatchType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartWith" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="startWith" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Parameters" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="parameters" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/ParametersCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="ParametersCollection" xmlItemName="add" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/ParameterCollectionElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ParameterCollectionElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="TemplateCollection" xmlItemName="add" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/TemplateCollectionElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="TemplateCollectionElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Filepath" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="filepath" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/6aded672-8fe8-44ba-8422-0e25a07f4071/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>