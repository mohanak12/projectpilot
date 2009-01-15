<xsl:stylesheet
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="1.0">
  <xsl:output method="xml"/>

  <xsl:template match="/report">
    <report>
      <testRun>
        <xsl:attribute name="startTime">
        </xsl:attribute>
        <xsl:attribute name="duration">
          <xsl:value-of select="testPackageRun/statistics/@duration"/>
        </xsl:attribute>
        <xsl:attribute name="version">
        </xsl:attribute>
        <suites>
          <suite>
            <xsl:attribute name="id">
            </xsl:attribute>
            <case>
              <xsl:attribute name="id">
              </xsl:attribute>
              <xsl:attribute name="startTime">
              </xsl:attribute>
              <xsl:attribute name="duration">
              </xsl:attribute>
              <xsl:attribute name="status">
              </xsl:attribute>
              <userStories>
                <userStory><xsl:text>F.SmsUI.RequestInfo</xsl:text></userStory>
                <userStory><xsl:text>F.SmsUI.ListSubsForTopic</xsl:text></userStory>
              </userStories>
            </case>
          </suite>
        </suites>
      </testRun>
    </report>
  </xsl:template>

</xsl:stylesheet>