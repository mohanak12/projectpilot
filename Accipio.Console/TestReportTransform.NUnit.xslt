<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml"/>

  <xsl:template match="/test-results">
    <report>
      <testRun>
        <xsl:attribute name="startTime">
          <xsl:value-of select="@date"/>
          <xsl:text>T</xsl:text>
          <xsl:value-of select="@time"/>
        </xsl:attribute>
        <xsl:attribute name="duration">
          <xsl:value-of select="./test-suite/@time"/>
        </xsl:attribute>
        <suites>
          <xsl:for-each select="//test-suite[(child::results/test-case)]">
            <suite>
              <xsl:attribute name="id">
                <xsl:value-of select="@name"/>
              </xsl:attribute>
              <xsl:for-each select=".//test-case">
                <case>
                  <xsl:attribute name="id">
                    <xsl:value-of select="@name"/>
                  </xsl:attribute>
                  <xsl:attribute name="startTime">
                    <xsl:value-of select="@startTime"/>
                  </xsl:attribute>
                  <xsl:attribute name="duration">
                    <xsl:value-of select="@time"/>
                  </xsl:attribute>
                  <xsl:attribute name="status">
                    <xsl:if test="@executed='False'">
                      <xsl:text>pending</xsl:text>
                    </xsl:if>
                    <xsl:if test="@success='True'">
                      <xsl:text>passed</xsl:text>
                    </xsl:if>
                    <xsl:if test="@success='False'">
                      <xsl:text>failed</xsl:text>
                    </xsl:if>
                  </xsl:attribute>
                  <userStories>
                    <!--
                    <xsl:for-each select="./categories/category">
                    -->
                      <userStory>
                        <xsl:value-of select="./categories/category/@name[1]"/>
                      </userStory>
                    <!--
                    </xsl:for-each>
                    -->
                  </userStories>
                  <message>
                    <xsl:for-each select="./failure/message">
                      <xsl:value-of select="."/>
                    </xsl:for-each>
                  </message>
                </case>
              </xsl:for-each>
            </suite>
          </xsl:for-each>
        </suites>
      </testRun>
    </report>
  </xsl:template>

</xsl:stylesheet>

