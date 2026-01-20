import React, { useState } from 'react'
import { Link } from 'react-router-dom'
import '../css/Navbar.css'
import logo from '../images/logo.svg'
import globe from '../images/globe.svg'
import add from '../images/add.svg'
import bell from '../images/bell.svg'
import user from '../images/user-profile-circle.svg'

function Navbar() {
  const [open, setOpen] = useState(false)
  const [openGlobe, setOpenGlobe] = useState(false)

  return (
    <>
      <div className="container">

        <nav>
          <div className="logo">
            <Link to="/">
              <img src={logo} alt="Bina24 Logo" />
              <h1>Bina 24</h1>
            </Link>
          </div>
            <ul className="navbar">
              <li><Link to="/buy-sell">Ev-alqı satqısı</Link></li>
              <li><Link to="/rent">Kirayə</Link></li>
              <li><Link to="/daily">Günlük</Link></li>
              <li><Link to="/roommate">Otaq yoldaşı</Link></li>
              <li><Link to="/complexes">Yaşayış kompleksləri</Link></li>
              <li><Link to="/mortgage">İpoteka</Link></li>
            </ul>

          <div className="right-side">
            <div className="language">
              <button onClick={() => {
                setOpenGlobe(!openGlobe)
              }}>
                <img src={globe} alt="" />
                AZ  
              </button>
              {openGlobe && (
                <div className="language-dropdown">
                  <ul>
                    <li> <Link to="/my-rent" onClick={() => setOpenGlobe(false)}>AZ</Link> </li>
                    <li> <Link to="/sell" onClick={() => setOpenGlobe(false)}>ENG</Link></li>
                    <li> <Link to="/sell" onClick={() => setOpenGlobe(false)}>RU</Link></li>
                  </ul>
                </div>
              )}
             
            </div>
            <div className="announcement">
              <button onClick={() => {
                setOpen(!open)
              }}>
                <img src={add} alt="" />
                Elan
              </button>
              {open && (
                <div className="announcement-dropdown">
                  <ul>
                    <li> <Link to="/my-rent" onClick={() => setOpen(false)}>Kirayə </Link> </li>
                    <li> <Link to="/sell" onClick={() => setOpen(false)}>Satıram</Link></li>
                  </ul>
                </div>
              )}
            </div>
            <div className="bell">
              <img src={bell} alt="" />
            </div>
            <div className="user">
              <img src={user} alt="" />
            </div>
          </div>
        </nav>
      </div>
    </>
  )
}

export default Navbar