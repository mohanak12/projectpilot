<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="html"/>

	<xsl:template match="/">
		
		<html>
			<h1>Business Actions</h1>
			<body>
				<table border="1">
					<tr bgcolor="#9acd32">
						<th align="left">Id</th>
						<th align="left">Description</th>
						<th align="left">Parameters</th>
					</tr>
					<xsl:for-each select="//action">
						<xsl:sort select="@id"/>
						<tr>
							<td>
								<xsl:value-of select="@id"/>
							</td>
							<td>
								<xsl:value-of select="description"/>
							</td>
							<td>
								<xsl:for-each select="parameter">
									<xsl:value-of select="@name"/>
									<xsl:text>( </xsl:text>
									<xsl:value-of select="@type"/>
									<xsl:text> )</xsl:text>
									<xsl:if test="position()!=last()">
										<xsl:text>, </xsl:text>
									</xsl:if>
								</xsl:for-each>
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>

	</xsl:template>
	
</xsl:stylesheet>
