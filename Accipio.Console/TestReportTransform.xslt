<xsl:stylesheet
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="1.0">
  <xsl:output method="xml"/>

  <xsl:template match="/report">
    <report>
      <testRun>
        <xsl:attribute name="startTime">
          <xsl:value-of select="testPackageRun/@startTime"/>
        </xsl:attribute>
        <xsl:attribute name="duration">
          <xsl:value-of select="testPackageRun/statistics/@duration"/>
        </xsl:attribute>
        <xsl:attribute name="version">
        </xsl:attribute>
        <suites>
          <xsl:for-each select="//testStepRun/testStep[@isTestCase='false']">
            <xsl:if test="metadata/entry/value='Fixture'">
              <suite>
                <xsl:attribute name="id">
                  <xsl:value-of select="@name"/>
                </xsl:attribute>
                  <xsl:for-each select="//testStepRun/testStep[@isTestCase='true']">
                  <case>
                    <xsl:attribute name="id">
                      <xsl:value-of select="@name"/>
                    </xsl:attribute>
                    <xsl:attribute name="startTime">
                      <xsl:value-of select="../@startTime"/>
                    </xsl:attribute>
                    <xsl:attribute name="duration">
                      <xsl:value-of select="../result/@duration"/>
                    </xsl:attribute>
                    <xsl:attribute name="status">
                      <xsl:value-of select="../result/outcome/@status"/>
                    </xsl:attribute>
                    <userStories>
                      <userStory>
                        <xsl:for-each select="metadata/entry[@key='UserStory']">
                          <xsl:value-of select="value"/>
                        </xsl:for-each>
                      </userStory>
                    </userStories>
                  </case>
                </xsl:for-each>
              </suite>
            </xsl:if>
          </xsl:for-each>
        </suites>
      </testRun>
    </report>
  </xsl:template>

</xsl:stylesheet>