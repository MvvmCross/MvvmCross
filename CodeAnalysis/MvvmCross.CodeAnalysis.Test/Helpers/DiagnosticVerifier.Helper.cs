using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views;

namespace MvvmCross.CodeAnalysis.Test
{
    /// <summary>
    /// Class for turning strings into documents and getting the diagnostics on them
    /// All methods are static
    /// </summary>
    public abstract partial class DiagnosticVerifier
    {
        private static readonly MetadataReference _corlibReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        private static readonly MetadataReference _systemCoreReference = MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);
        private static readonly MetadataReference _cSharpSymbolsReference = MetadataReference.CreateFromFile(typeof(CSharpCompilation).Assembly.Location);
        private static readonly MetadataReference _codeAnalysisReference = MetadataReference.CreateFromFile(typeof(Compilation).Assembly.Location);
        private static readonly MetadataReference _mvvmCrossCoreReference = MetadataReference.CreateFromFile(typeof(MvxViewPresenter).Assembly.Location);
        private static readonly MetadataReference _mvvmCrossDroidReference = MetadataReference.CreateFromFile(typeof(MvxActivity).Assembly.Location);
        private static readonly MetadataReference _componentModelReference = MetadataReference.CreateFromFile(typeof(INotifyPropertyChanged).Assembly.Location);
        private static readonly MetadataReference _objectModelReference = MetadataReference.CreateFromFile(GetCorrectObjectModelPath());

        private static string GetCorrectObjectModelPath()
        {
            var winDir = Environment.GetEnvironmentVariable("windir");

            if (winDir != null)
            {
                var gacObjectModelPath = Path.Combine(winDir, @"Microsoft.NET\assembly\GAC_MSIL\System.ObjectModel");
                var finalPath = Directory.GetDirectories(gacObjectModelPath).First();

                return Directory.GetFiles(finalPath).First();
            }
            throw new ArgumentException("You don't have ObjectModel inside your GAC! Fix your environment.");
        }

        internal static string DefaultFilePathPrefix = "Test";
        internal static string CSharpDefaultFileExt = "cs";
        internal static string TestCoreProjectName = "CoreTestProject";
        internal static string TestDroidProjectName = "DroidTestProject";
        private static ProjectId _coreProjectId;
        private static ProjectId _droidProjectId;

        #region  Get Diagnostics

        /// <summary>
        /// Given classes in the form of strings, their language, and an IDiagnosticAnlayzer to apply to it, return the diagnostics found in the string after converting it to a document.
        /// </summary>
        /// <param name="fileSources">Classes in the form of MvxTestFileSources</param>
        /// <param name="analyzer">The analyzer to be run on the sources</param>
        /// <returns>An IEnumerable of Diagnostics that surfaced in the source code, sorted by Location</returns>
        private static Diagnostic[] GetSortedDiagnostics(MvxTestFileSource[] fileSources, DiagnosticAnalyzer analyzer)
        {
            return GetSortedDiagnosticsFromDocuments(analyzer, GetDocuments(fileSources));
        }

        /// <summary>
        /// Given an analyzer and a document to apply it to, run the analyzer and gather an array of diagnostics found in it.
        /// The returned diagnostics are then ordered by location in the source document.
        /// </summary>
        /// <param name="analyzer">The analyzer to run on the documents</param>
        /// <param name="documents">The Documents that the analyzer will be run on</param>
        /// <returns>An IEnumerable of Diagnostics that surfaced in the source code, sorted by Location</returns>
        protected static Diagnostic[] GetSortedDiagnosticsFromDocuments(DiagnosticAnalyzer analyzer, Document[] documents)
        {
            var projects = new HashSet<Project>();
            foreach (var document in documents)
            {
                projects.Add(document.Project);
            }

            var diagnostics = new List<Diagnostic>();
            foreach (var project in projects)
            {
                var compilationWithAnalyzers = project.GetCompilationAsync().Result.WithAnalyzers(ImmutableArray.Create(analyzer));
                var diags = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result;
                foreach (var diag in diags)
                {
                    if (diag.Location == Location.None || diag.Location.IsInMetadata)
                    {
                        diagnostics.Add(diag);
                    }
                    else
                    {
                        diagnostics.AddRange(from document in documents select document.GetSyntaxTreeAsync().Result into tree where tree == diag.Location.SourceTree select diag);
                    }
                }
            }

            var results = SortDiagnostics(diagnostics);
            diagnostics.Clear();
            return results;
        }

        /// <summary>
        /// Sort diagnostics by location in source document
        /// </summary>
        /// <param name="diagnostics">The list of Diagnostics to be sorted</param>
        /// <returns>An IEnumerable containing the Diagnostics in order of Location</returns>
        private static Diagnostic[] SortDiagnostics(IEnumerable<Diagnostic> diagnostics)
        {
            return diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
        }

        #endregion

        #region Set up compilation and documents

        /// <summary>
        /// Given an array of strings as sources and a language, turn them into a project and return the documents and spans of it.
        /// </summary>
        /// <param name="fileSources">Classes in the form of MvxTestFileSources</param>
        /// <returns>A Tuple containing the Documents produced from the sources and their TextSpans if relevant</returns>
        protected static Document[] GetDocuments(MvxTestFileSource[] fileSources)
        {
            var solution = CreateSolution(fileSources);

            var documents = new List<Document>();
            foreach (var projectId in solution.ProjectIds)
            {
                var project = solution.GetProject(projectId);
                var projectDocuments = project.Documents.ToArray();

                if (fileSources.Count(f => f.ProjType == (projectId == _coreProjectId ? MvxProjType.Core : MvxProjType.Droid)) != projectDocuments.Length)
                {
                    throw new SystemException("Amount of sources did not match amount of Documents created");
                }
                documents.AddRange(projectDocuments);
            }

            return documents.ToArray();
        }

        /// <summary>
        /// Create a Document from a string through creating a project that contains it.
        /// </summary>
        /// <param name="fileSources">Classes in the form of MvxTestFileSources</param>
        /// <param name="fileSource">The file the should be added last, aldo also to be returned</param>
        /// <returns>A Document created from the source string</returns>
        protected static Document CreateDocument(MvxTestFileSource[] fileSources, MvxTestFileSource fileSource)
        {
            var documents = fileSources?.ToList() ?? new List<MvxTestFileSource>();
            documents.Add(fileSource);

            return CreateSolution(documents.ToArray())
                .GetProject(fileSource.ProjType == MvxProjType.Core ? _coreProjectId : _droidProjectId).Documents.Last();
        }

        /// <summary>
        /// Create a project using the inputted strings as sources.
        /// </summary>
        /// <param name="fileSources">Classes in the form of MvxTestFileSources</param>
        /// <returns>A Solution created out of the Documents created from the source strings</returns>
        private static Solution CreateSolution(MvxTestFileSource[] fileSources)
        {
            string fileNamePrefix = DefaultFilePathPrefix;

            _coreProjectId = ProjectId.CreateNewId(debugName: TestCoreProjectName);
            _droidProjectId = ProjectId.CreateNewId(debugName: TestDroidProjectName);

            var solution = new AdhocWorkspace()
                .CurrentSolution
                .AddProject(_coreProjectId, TestCoreProjectName, TestCoreProjectName, LanguageNames.CSharp)
                .AddProject(_droidProjectId, TestDroidProjectName, TestDroidProjectName, LanguageNames.CSharp)
                .AddMetadataReference(_coreProjectId, _corlibReference)
                .AddMetadataReference(_coreProjectId, _systemCoreReference)
                .AddMetadataReference(_coreProjectId, _cSharpSymbolsReference)
                .AddMetadataReference(_coreProjectId, _codeAnalysisReference)
                .AddMetadataReference(_coreProjectId, _mvvmCrossCoreReference)
                .AddMetadataReference(_coreProjectId, _componentModelReference)
                .AddMetadataReference(_coreProjectId, _objectModelReference)
                .AddMetadataReference(_droidProjectId, _corlibReference)
                .AddMetadataReference(_droidProjectId, _systemCoreReference)
                .AddMetadataReference(_droidProjectId, _cSharpSymbolsReference)
                .AddMetadataReference(_droidProjectId, _codeAnalysisReference)
                .AddMetadataReference(_droidProjectId, _mvvmCrossCoreReference)
                .AddMetadataReference(_droidProjectId, _mvvmCrossDroidReference)
                .AddMetadataReference(_droidProjectId, _componentModelReference)
                .AddMetadataReference(_droidProjectId, _objectModelReference)
                .AddProjectReference(_droidProjectId, new ProjectReference(_coreProjectId));

            int count = 0;
            foreach (var fileSource in fileSources)
            {
                var newFileName = fileNamePrefix + count + "." + CSharpDefaultFileExt;
                var documentId = DocumentId.CreateNewId(fileSource.ProjType == MvxProjType.Core ? _coreProjectId : _droidProjectId, debugName: newFileName);
                solution = solution.AddDocument(documentId, newFileName, SourceText.From(fileSource.Source));
                count++;
            }

            return solution;
        }
        #endregion
    }
}

