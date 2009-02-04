<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:g="http://www.gallio.org/"
                xmlns="http://projectpilot/AccipioTestRunReport.xsd">
  <xsl:output method="xml"/>

  <xsl:template match="g:report">
    <report>
      <testRun>
        <xsl:attribute name="startTime">
          <xsl:value-of select="g:testPackageRun/@startTime"/>
        </xsl:attribute>
        <xsl:attribute name="duration">
          <xsl:value-of select="g:testPackageRun/g:statistics/@duration"/>
        </xsl:attribute>
        <suites>
          <xsl:for-each select="g:testPackageRun//g:testStepRun/g:testStep[@isTestCase='false']">
            <xsl:if test="g:metadata/g:entry/g:value='Fixture'">
              <suite>
                <xsl:attribute name="id">
                  <xsl:value-of select="@name"/>
                </xsl:attribute>
                <xsl:for-each select="../g:children//g:testStepRun/g:testStep[@isTestCase='true']">
                  <case>
                    <xsl:attribute name="id">
                      <xsl:value-of select="@name"/>
                    </xsl:attribute>
                    <xsl:attribute name="startTime">
                      <xsl:value-of select="../@startTime"/>
                    </xsl:attribute>
                    <xsl:attribute name="duration">
                      <xsl:value-of select="../g:result/@duration"/>
                    </xsl:attribute>
                    <xsl:attribute name="status">
                      <xsl:value-of select="../g:result/g:outcome/@status"/>
                    </xsl:attribute>
                    <userStories>
                      <userStory>
                        <xsl:for-each select="g:metadata/g:entry[@key='UserStory']">
                          <xsl:value-of select="g:value"/>
                        </xsl:for-each>
                      </userStory>
                    </userStories>
                    <xsl:if test="../g:result/g:outcome[@status='failed']">
                      <error>
                        <xsl:for-each select="../g:testLog/g:streams/g:stream[@name='Failures']//g:text">
                          <xsl:value-of select="."/>
                        </xsl:for-each>
                      </error>
                    </xsl:if>
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