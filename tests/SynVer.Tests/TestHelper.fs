module SynVer.TestHelper
open SynVer
open System.Reflection

open System.IO
/// path combine
let (</>) = (fun a b -> Path.Combine(a, b))
let nlJoin (lines:string seq)=System.String.Join("\n",lines)
let wTrim (str:string) = str.Trim([| '\n';'\t';' ';'\r' |])
let asm = Assembly.GetExecutingAssembly()
let testPath= Path.GetDirectoryName asm.Location
let exampleProjectsPath = Path.GetFullPath( testPath </> ".."</>".."</>".."</>"ExampleProjects" )
let packagesPath = Path.GetFullPath( testPath </> ".."</>".."</>".."</>".."</> "packages")
let exampleProjectsLibPath = exampleProjectsPath </> "lib"
// since all the dll-s have unique names, they can be loaded at the same time
let csharp = exampleProjectsLibPath </> "Csharp.dll" |> Assembly.LoadFile
let csharp2 = exampleProjectsLibPath </> "Csharp2.dll" |> Assembly.LoadFile
let csharpWithAttribute = exampleProjectsLibPath </> "CsharpWithAttribute.dll" |> Assembly.LoadFile
let enum = exampleProjectsLibPath </> "Enum.dll" |> Assembly.LoadFile
let enum2 = exampleProjectsLibPath </> "Enum2.dll" |> Assembly.LoadFile
let enum2txt = exampleProjectsPath </> "src" </> "Enum2.txt" |> File.ReadAllLines |>nlJoin |> wTrim
let enum3 = exampleProjectsLibPath </> "Enum3.dll" |> Assembly.LoadFile
let enum3txt = exampleProjectsPath </> "src" </> "Enum3.txt" |> File.ReadAllLines |>nlJoin |> wTrim
let fsharp = exampleProjectsLibPath </> "Fsharp.dll" |> Assembly.LoadFile
let fsharp2 = exampleProjectsLibPath </> "Fsharp2.dll" |> Assembly.LoadFile
let fsharp2txt = exampleProjectsPath </> "src" </> "Fsharp2.txt" |> File.ReadAllLines |>nlJoin |> wTrim
let arguAssembly= packagesPath </>"Argu"</>"lib"</> "net40" </>"Argu.dll" |> Assembly.LoadFile
let chironAssembly= packagesPath </>"Chiron"</>"lib"</> "net40" </>"Chiron.dll" |> Assembly.LoadFile

module Types=
  type UnionCaseWithName = Foo of num: int * diff:float | Bar of diff:float
  type Union = FooBar | Foo | Bar 
  type UnionWithParam = Foo of (int * float) | Bar of float
  [<System.FlagsAttribute>]
  type EnumType = FooBar=0 | Foo=1 | Bar =2
  type RecordType = { Foo:int}
  type UnionWithParamNames = Foo of num: int * diff:float | Bar of diff:float
  let CSharpStructType = csharp.ExportedTypes |> Seq.find (fun t-> t.Name= "Struct")

  type FsharpStruct =
   struct
      val x: float
   end
  
  let ModuleT= fsharp.ExportedTypes |> Seq.find (fun t-> t.Name="Module")

module Assemblies=
  [<CompiledName("Bump")>]
  let bump verNr released modified =
    SurfaceArea.bump verNr 
      (SurfaceArea.ofAssembly released) 
      (SurfaceArea.ofAssembly modified)   

  [<CompiledName("Diff")>]
  let diff released modified =
    SurfaceArea.diff  
      (SurfaceArea.ofAssembly released) 
      (SurfaceArea.ofAssembly modified)