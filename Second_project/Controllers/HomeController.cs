using Second_project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Second_project.Controllers
{
    public class HomeController : Controller
    {
     private Premier_League1Entities db = new Premier_League1Entities ();
        // GET: Homme
        public ActionResult Index(string playerPosition, string searchString)
        {
            //LINQ query to return all players from the database
            var players = from p in db.TablePremierLeague1
                          select p;

            //players position list search
            //first put players position into the position dropdown list
            var PositionList = new List<string>();

            //LINQ query to retrive players position from db to populate the players position dropdown list 
            var Positionquery = from p in db.TablePremierLeague1
                                orderby p.Position
                                select p.Position;

            //adding player position to the position list without duplicates
            PositionList.AddRange(Positionquery.Distinct());

            //put the list position into the viewBag to pass it to the Index view
            ViewBag.playerPosition = new SelectList(PositionList);


            //if there is a value in the playerPosition string, then search for that position
            if (!String.IsNullOrEmpty(playerPosition))
            {
                players = players.Where(x => x.Position == playerPosition);
            }

            //search player
            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(s => s.Name.Contains(searchString));
            }

            return View(players);

        }

        public ActionResult Like(int id)
        {
            int currentLikes;
            TablePremierLeague1 player = db.TablePremierLeague1.Find(id);

            if (player.Likes == null)
            {
                currentLikes = 0;
            }
            else
            {
                currentLikes = (int)player.Likes;
            }

            player.Likes = currentLikes + 1;

           if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Dislike(int id)
        {
            int currentDislikes;
            TablePremierLeague1 player = db.TablePremierLeague1.Find(id);

            if (player.Dislikes == null)
            {
                currentDislikes = 0;
            }
            else
            {
                currentDislikes = (int)player.Dislikes;
            }
             
            player.Dislikes = currentDislikes + 1;

            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


     
        // the question mark ? means if someone doens not pass a parameter the code wont cratch here, although it may cratch somewhere else
        public ActionResult Details(int? id)
        {
            //if the player is missing...
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get the Player
            TablePremierLeague1 player = db.TablePremierLeague1.Find(id);

            //if the movie id was not found in the database
            if (player == null)
            {
                return HttpNotFound();
            }

                return View(player);
        }


        // using Get method to add record create new 
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TablePremierLeague1 player)
        {
            if (player.Picture == null)
            {
                player.Picture = "http://cliparts.co/cliparts/kcK/np7/kcKnp7q7i.jpg";
            }

            if (ModelState.IsValid)
            {


                //add player to db using data returned from view
                db.TablePremierLeague1.Add(player);
                //save changes to db
                db.SaveChanges();
                // go back to the main page
                return RedirectToAction("Index");
            }
            return View(player);
        }


        [HttpGet]
        // the question mark ? means if someone doens not pass a parameter the code wont cratch here, although it may cratch somewhere else
        public ActionResult Edit(int? id)
        {

            //if the player is missing...
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            //get the movie
            TablePremierLeague1 player = db.TablePremierLeague1.Find(id);


            //if the movie id was not found in the database
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        public ActionResult Edit(TablePremierLeague1 player)
        {
            if (player.Picture == null)
            {
                player.Picture = "http://cliparts.co/cliparts/kcK/np7/kcKnp7q7i.jpg";
            }

            if (ModelState.IsValid)
            {
                //get the edited data 
                db.Entry(player).State = EntityState.Modified;

                //save changes to the DB
                db.SaveChanges();

                //go back to the index
                return RedirectToAction("Index");
            }
            return View(player);
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            //if the movie is missing...
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            //get the movie
            TablePremierLeague1 player = db.TablePremierLeague1.Find(id);


            //if the movie id was not found in the database
            if (player == null)
            {
                return HttpNotFound();
            }

            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            TablePremierLeague1 player = db.TablePremierLeague1.Find(id);
            db.TablePremierLeague1.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }


    }
}