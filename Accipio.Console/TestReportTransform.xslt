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
          <xsl:for-each select="testPackageRun/testStepRun/children/testStepRun/children/testStepRun/children/testStepRun">
            <suite>
              <xsl:attribute name="id">
                <xsl:value-of select="testStep/@name"/>
              </xsl:attribute>
                <xsl:for-each select="children">
                <case>
                  <xsl:attribute name="id">
                    <xsl:value-of select="testStepRun/testStep/@name"/>
                  </xsl:attribute>
                  <xsl:attribute name="startTime">
                  </xsl:attribute>
                  <xsl:attribute name="duration">
                    <xsl:value-of select="testStepRun/result/@duration"/>
                  </xsl:attribute>
                  <xsl:attribute name="status">
                  </xsl:attribute>
                  <userStories>
                    <userStory><xsl:text>F.SmsUI.RequestInfo</xsl:text></userStory>
                    <userStory><xsl:text>F.SmsUI.ListSubsForTopic</xsl:text></userStory>
                  </userStories>
                </case>
              </xsl:for-each>
            </suite>
          </xsl:for-each>
        </suites>
      </testRun>
    </report>
  </xsl:template>

</xsl:stylesheet>