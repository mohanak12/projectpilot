using System;
using System.Collections.Generic;
using System.IO;
using Flubu.Builds.VSSolutionBrowsing;

namespace Flubu.Builds
{
    /// <summary>
    /// Registry of products of a build. The products will be collected, packaged and sent to the build server's artifact directory
    /// after a successful build.
    /// </summary>
    /// <typeparam name="TRunner">The concrete type of the runner.</typeparam>
    public class BuildProductsRegistry<TRunner>
        where TRunner : BuildRunner<TRunner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProductsRegistry{TRunner}"/> class.
        /// </summary>
        /// <param name="runner">The build runner to use.</param>
        public BuildProductsRegistry(TRunner runner)
        {
            this.runner = runner;
        }

        /// <summary>
        /// Gets a value indicating whether this instance of build products registry has any products.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has any products; otherwise, <c>false</c>.
        /// </value>
        public bool HasAnyProducts
        {
            get { return buildProducts.Count > 0; }
        }

        /// <summary>
        /// Adds all VisualStudio Web projects registered in the solution to the list of build products.
        /// </summary>
        /// <param name="buildPartId">The ID of the build part Web projects will belong to.</param>
        /// <returns>
        /// This same instance of the <see cref="BuildProductsRegistry{TRunner}"/>.
        /// </returns>
        public BuildProductsRegistry<TRunner> AddAllWebProjects(string buildPartId)
        {
            foreach (VSProjectExtendedInfo projectExtendedInfo in runner.ProjectExtendedInfos.Values)
            {
                if (projectExtendedInfo.IsWebProject)
                    AddProject(buildPartId, projectExtendedInfo.ProjectInfo.ProjectName);
            }

            return this;
        }

        /// <summary>
        /// Adds the specified directory to the list of build products.
        /// </summary>
        /// <param name="buildPartId">The ID of the build part this directory belongs to.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>
        /// This same instance of the <see cref="BuildProductsRegistry{TRunner}"/>.
        /// </returns>
        public BuildProductsRegistry<TRunner> AddDirectory(string buildPartId, string directoryPath)
        {
            buildProducts.Add(
                new SimpleBuildProduct<TRunner>(
                    buildPartId, 
                    directoryPath, 
                    Path.GetFileName(directoryPath), 
                    null, 
                    null));
            return this;
        }

        /// <summary>
        /// Adds the specified directory to the list of build products.
        /// </summary>
        /// <param name="buildPartId">The ID of the build part this directory belongs to.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="inclusionPattern">The inclusion regular expression pattern for files.</param>
        /// <param name="exclusionPattern">The exclusion regular expression pattern for files.</param>
        /// <returns>
        /// This same instance of the <see cref="BuildProductsRegistry{TRunner}"/>.
        /// </returns>
        public BuildProductsRegistry<TRunner> AddDirectory(
            string buildPartId,
            string directoryPath,
            string inclusionPattern,
            string exclusionPattern)
        {
            buildProducts.Add(
                new SimpleBuildProduct<TRunner>(
                    buildPartId, 
                    directoryPath, 
                    Path.GetFileName(directoryPath), 
                    inclusionPattern, 
                    exclusionPattern));
            return this;
        }

        /// <summary>
        /// Adds the specified VisualStudio project to the list of build products.
        /// </summary>
        /// <param name="buildPartId">The ID of the build part this project belongs to.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <returns>
        /// This same instance of the <see cref="BuildProductsRegistry{TRunner}"/>.
        /// </returns>
        public BuildProductsRegistry<TRunner> AddProject(string buildPartId, string projectName)
        {
            VSProjectInfo projectInfo = runner.Solution.FindProjectByName(projectName);

            if (runner.ProjectExtendedInfos.ContainsKey(projectName))
            {
                VSProjectExtendedInfo extendedInfo = runner.ProjectExtendedInfos[projectName];
                if (extendedInfo.IsWebProject)
                {
                    buildProducts.Add(
                        new WebApplicationBuildProduct<TRunner>(buildPartId, projectInfo.ProjectDirectoryPath, projectName));
                    return this;
                }
            }

            buildProducts.Add(
                new SimpleBuildProduct<TRunner>(
                    buildPartId, 
                    Path.Combine(projectInfo.ProjectDirectoryPath, runner.GetProjectOutputPath(projectName)), 
                    projectName, 
                    null, 
                    null));

            return this;
        }

        public void CopyProducts(string packageDirectory)
        {
            foreach (BuildProduct<TRunner> product in buildProducts)
                product.CopyProductFiles(runner, packageDirectory);
        }

        /// <summary>
        /// Lists all of files which belong to the specified product parts.
        /// </summary>
        /// <param name="ids">The IDs of product parts. If the list is empty, the method will use all available product parts.</param>
        /// <returns>A list of files files which belong to the specified product parts.</returns>
        public IEnumerable<string> ListFilesForProductParts(string[] ids)
        {
            List<string> files = new List<string>();

            foreach (BuildProduct<TRunner> buildProduct in buildProducts)
            {
                int index = 0;

                if (ids.Length > 0)
                {
                    index = Array.FindIndex(
                        ids,
                        match => 0 == String.Compare(
                                          match,
                                          buildProduct.ProductPartId,
                                          StringComparison.OrdinalIgnoreCase));
                }

                // if this product is in the list of product parts, use its files
                if (index != -1)
                    files.AddRange(buildProduct.ListCopiedFiles());
            }

            return files;
        }

        private readonly TRunner runner;
        private List<BuildProduct<TRunner>> buildProducts = new List<BuildProduct<TRunner>>();
    }
}