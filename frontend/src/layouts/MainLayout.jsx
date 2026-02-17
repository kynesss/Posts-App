import { AppBar, Box, Toolbar, Typography, Button } from '@mui/material'
import { NavLink, Outlet } from 'react-router-dom'

const MainLayout = () => {

  const navItems = [
    { label: "Home", to: "/", end: true },
    { label: "Posts", to: "/posts" },
    { label: "About", to: "/about" }
  ];


  return (
    <Box>
      <AppBar position='static'>
        <Toolbar sx={{ gap: 1}}>
          <Typography variant='h6' color="inherit">Posts Comments App</Typography>
          { navItems.map((item) => (
            <Button key={item.to} 
              color="inherit" 
              component={NavLink} 
              to={item.to} 
              end={item.end} 
              className={({ isActive }) => (isActive ? 'active' : '')}
              sx={{
                '&.active': {
                  bgcolor: 'rgba(255,255,255,0.2)',
                  fontWeight: 700,
                },
              }}
            >
              { item.label }
            </Button>
          ))}
        </Toolbar>
      </AppBar>

      <Box sx={{ p: 3}}>
        <Outlet />
      </Box>
    </Box>
  )
}

export default MainLayout