<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">
  <html>
  <head>
	<style>
	table, th, td {
		border: 1px solid black;
		border-collapse:collapse;
	}
	th, td {
		padding: 5px;
	}
</style>
</head>
  <body style="font-family:'Lucida Console', Monaco, monospace" >
    <h2>Lista delle password</h2>
    <table border="1">
      <tr bgcolor="#9acd32">
        <th>Sito web</th>
        <th>Nome Utente</th>
		<th>Matrice</th>
		<th>Password</th>
      </tr><xsl:for-each select="ListaPassword/Password">
    <tr>
      <td><xsl:value-of select="sito"/></td>
      <td><xsl:value-of select="nomeUtente"/></td>
	  <td><xsl:value-of select="matrice"/></td>
	  <td><xsl:value-of select="pw"/></td>
    </tr>
    </xsl:for-each>
    </table>
  </body>
  </html>
</xsl:template>
</xsl:stylesheet>