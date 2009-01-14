<xsl:stylesheet
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="1.0">
  <xsl:output method="xml"/>

  <xsl:template match="/report">
    <report>
      <testRun>
        <xsl:text>testCount=</xsl:text>
        <xsl:value-of select="testPackageRun/statistics/@testCount"/>
        <xsl:text>, duration=</xsl:text>
        <xsl:value-of select="testPackageRun/statistics/@duration"/>
        <xsl:text>, passedCount=</xsl:text>
        <xsl:value-of select="testPackageRun/statistics/@passedCount"/>
        <xsl:text>, failedCount=</xsl:text>
        <xsl:value-of select="testPackageRun/statistics/@failedCount"/>
        <xsl:text>, inconclusiveCount=</xsl:text>
        <xsl:value-of select="testPackageRun/statistics/@inconclusiveCount"/>
        <xsl:text>, skippedCount=</xsl:text>
        <xsl:value-of select="testPackageRun/statistics/@skippedCount"/>
      </testRun>
    </report>
  </xsl:template>

</xsl:stylesheet>