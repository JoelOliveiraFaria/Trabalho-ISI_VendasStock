import { useState, useEffect } from 'react'
import axios from 'axios'
import { BarChart3, Package, TrendingUp, AlertTriangle } from 'lucide-react'
import './App.css'
import SalesChart from './components/SalesChart'
import StockTable from './components/StockTable'

const API_URL = 'https://localhost:7047/api'

function App() {
  const [salesStats, setSalesStats] = useState(null)
  const [stockStats, setStockStats] = useState(null)
  const [lowStock, setLowStock] = useState([])
  const [topProducts, setTopProducts] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    fetchData()
  }, [])

  const fetchData = async () => {
    try {
      setLoading(true)
      setError(null)

      const [sales, stock, low, products] = await Promise.all([
        axios.get(`${API_URL}/sales/stats`),
        axios.get(`${API_URL}/warehousestock/stats`),
        axios.get(`${API_URL}/warehousestock/low-stock`),
        axios.get(`${API_URL}/sales/by-product`)
      ])

      // 🔍 DEBUG
      console.log('📊 Dados recebidos:')
      console.log('Sales:', sales.data)
      console.log('Stock:', stock.data)
      console.log('Top Products:', products.data)
      console.log('Low Stock:', low.data)

      setSalesStats(sales.data)
      setStockStats(stock.data)
      setLowStock(low.data)
      setTopProducts(products.data)
      
    } catch (error) {
      console.error('❌ Erro:', error)
      setError('Erro ao carregar dados da API')
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return (
      <div className="loading">
        <div className="spinner"></div>
        <p>A carregar dados...</p>
      </div>
    )
  }

  if (error) {
    return (
      <div className="loading">
        <p style={{ color: '#ef4444' }}>❌ {error}</p>
        <button onClick={fetchData} className="refresh-btn">🔄 Tentar Novamente</button>
      </div>
    )
  }

  return (
    <div className="dashboard">
      <header>
        <h1>📊 Sales & Warehouse Dashboard</h1>
        <button onClick={fetchData} className="refresh-btn">🔄 Atualizar</button>
      </header>

      {/* KPI Cards */}
      <div className="kpi-grid">
        <div className="kpi-card blue">
          <div className="kpi-icon"><BarChart3 size={32} /></div>
          <div className="kpi-content">
            <h3>Total de Vendas</h3>
            <p className="kpi-value">{salesStats?.totalSales || 0}</p>
            <span className="kpi-subtitle">Registos</span>
          </div>
        </div>

        <div className="kpi-card green">
          <div className="kpi-icon"><TrendingUp size={32} /></div>
          <div className="kpi-content">
            <h3>Receita Total</h3>
            <p className="kpi-value">
              €{salesStats?.totalRevenue 
                ? (salesStats.totalRevenue / 1000000).toFixed(2) + 'M'
                : '0.00M'
              }
            </p>
            <span className="kpi-subtitle">Milhões de Euros</span>
          </div>
        </div>

        <div className="kpi-card purple">
          <div className="kpi-icon"><Package size={32} /></div>
          <div className="kpi-content">
            <h3>Total de Produtos</h3>
            <p className="kpi-value">{stockStats?.totalProducts || 0}</p>
            <span className="kpi-subtitle">Em Stock</span>
          </div>
        </div>

        <div className="kpi-card red">
          <div className="kpi-icon"><AlertTriangle size={32} /></div>
          <div className="kpi-content">
            <h3>Stock Baixo</h3>
            <p className="kpi-value">{stockStats?.lowStockCount || 0}</p>
            <span className="kpi-subtitle">Produtos</span>
          </div>
        </div>
      </div>

      {/* Charts */}
      <div className="charts-grid">
        <div className="chart-card">
          <h2>🔥 Top 10 Produtos por Receita</h2>
          <SalesChart data={topProducts || []} />
        </div>

        <div className="chart-card">
          <h2>⚠️ Produtos com Stock Baixo</h2>
          <StockTable data={lowStock.slice(0, 10) || []} />
        </div>
      </div>
    </div>
  )
}

export default App
