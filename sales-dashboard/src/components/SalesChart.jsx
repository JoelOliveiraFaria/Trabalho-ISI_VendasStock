import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Legend } from 'recharts'

/**
 * Componente de gráfico de barras para exibir top produtos por receita.
 * Usa biblioteca Recharts para visualização de dados.
 */
function SalesChart({ data }) {
  return (
    // Container responsivo que ajusta ao tamanho do pai
    <ResponsiveContainer width="100%" height={300}>
      <BarChart data={data}>
        {/* Grade de fundo com linhas tracejadas */}
        <CartesianGrid strokeDasharray="3 3" stroke="#333" />
        
        {/* Eixo X: nomes dos produtos */}
        <XAxis dataKey="product" tick={{ fill: '#fff' }} />
        
        {/* Eixo Y: valores de receita */}
        <YAxis tick={{ fill: '#fff' }} />
        
        {/* Tooltip personalizado ao passar o mouse */}
        <Tooltip 
          contentStyle={{ backgroundColor: '#1e1e1e', border: '1px solid #333' }}
          formatter={(value) => `€${value.toFixed(2)}`} // Formata valor como moeda
        />
        
        {/* Legenda do gráfico */}
        <Legend />
        
        {/* Barra de receita em verde */}
        <Bar dataKey="revenue" fill="#4ade80" name="Receita" />
      </BarChart>
    </ResponsiveContainer>
  )
}

export default SalesChart
