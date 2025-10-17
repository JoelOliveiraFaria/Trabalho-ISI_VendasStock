import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Legend } from 'recharts'

function SalesChart({ data }) {
  return (
    <ResponsiveContainer width="100%" height={300}>
      <BarChart data={data}>
        <CartesianGrid strokeDasharray="3 3" stroke="#333" />
        <XAxis dataKey="product" tick={{ fill: '#fff' }} />
        <YAxis tick={{ fill: '#fff' }} />
        <Tooltip 
          contentStyle={{ backgroundColor: '#1e1e1e', border: '1px solid #333' }}
          formatter={(value) => `€${value.toFixed(2)}`}
        />
        <Legend />
        <Bar dataKey="revenue" fill="#4ade80" name="Receita" />
      </BarChart>
    </ResponsiveContainer>
  )
}

export default SalesChart
