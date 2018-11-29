namespace Common
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Reflection;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Web.Services.Description;
    using Microsoft.CSharp;

    public static  class WebServiceKit
    {
        #region InvokeWebService 动态调用web服务
        /// <summary>
        ///  动态调用web服务
        /// </summary>
        /// <param name="url">web服务地址</param>
        /// <param name="methodName">调用方法名称</param>
        /// <param name="args">方法参数</param>
        /// <returns>代理实体类</returns>
        public static object InvokeWebService(string url, string methodName, params object[] args)
        {
            return InvokeWebService(url, null, methodName, args);
        }

        private static object InvokeWebService(string url, string className, string methodName, params object[] args)
        {
            const string @nameSpace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if (string.IsNullOrEmpty(className))
            {
                className = GetWsClassName(url);
            }

            try
            {
                //获取WSDL
                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(url + "?WSDL");
                ServiceDescription serviceDescription = ServiceDescription.Read(stream);
                ServiceDescriptionImporter serviceDescriptionImporter = new ServiceDescriptionImporter();
                serviceDescriptionImporter.AddServiceDescription(serviceDescription, string.Empty, string.Empty);
                CodeNamespace codeNamespace = new CodeNamespace(@nameSpace);

                //生成客户端代理类代码
                CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
                codeCompileUnit.Namespaces.Add(codeNamespace);
                serviceDescriptionImporter.Import(codeNamespace, codeCompileUnit);
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                #pragma warning disable 618,612
                ICodeCompiler codeCompiler = codeProvider.CreateCompiler();
                #pragma warning restore 618,612

                //设定编译参数
                CompilerParameters compilerParameters = new CompilerParameters
                                                            {
                                                                GenerateExecutable = false,
                                                                GenerateInMemory = true
                                                            };
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.XML.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Web.Services.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults compilerResults = codeCompiler.CompileAssemblyFromDom(compilerParameters, codeCompileUnit);
                if (compilerResults.Errors.HasErrors)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (CompilerError compilerError in compilerResults.Errors)
                    {
                        stringBuilder.Append(compilerError.ToString());
                        stringBuilder.Append(Environment.NewLine);
                    }
                    throw new Exception(stringBuilder.ToString());
                }

                //生成代理实例，并调用方法
                Assembly assembly = compilerResults.CompiledAssembly;
                Type type = assembly.GetType(@nameSpace + "." + className, true, true);
                object obj = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod(methodName);

                return methodInfo.Invoke(obj, args);
            }
            catch
            {
                return null;
            }
        }

        private static string GetWsClassName(string webServiceUrl)
        {
            string[] parts = webServiceUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
        #endregion
    }
}
