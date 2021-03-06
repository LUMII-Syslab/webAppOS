/* Generated By:JavaCC: Do not edit this line. MMDParser.java */
package lv.lumii.tda.kernel.mmdparser;

import lv.lumii.tda.raapi.RAAPI;

public class MMDParser implements MMDParserConstants {
  public static boolean debug = false;
  private RAAPI repository;
/*  public static void main(String args[]) throws ParseException {
    MMDParser p = new MMDParser(System.in);
    p.Start();
  }*/

  private boolean onlyCheckSyntax = true;
  private StringBuffer loadErrors = null;

  private String removeQuotes(String s)
  {
    if ((s.length()>=2) && (s.codePointAt(0)==0x22) && (s.codePointAt(s.length()-1)==0x22))
        return s.substring(1, s.length()-1);
    if ((s.length()>=2) && (s.codePointAt(0)==0x201C) && (s.codePointAt(s.length()-1)==0x201D))
        return s.substring(1, s.length()-1);
    if (s.startsWith("'") && s.endsWith("'"))
        return s.substring(1, s.length()-1);
    else
        return s;
  }

  public String checkSyntax()
  {
    onlyCheckSyntax = true;
    try
    {
        repository = null;
        Start();
        return null;
        } catch (ParseException e1) {
          return e1.toString();
        }
  }
    // returns null if OK, or error description
  public String loadMMD(RAAPI _repository)
  {
    onlyCheckSyntax = false;
    repository = _repository;
    loadErrors = new StringBuffer();
    try
    {
        Start();
        if (loadErrors.length()==0) {
          loadErrors = null;
              repository = null;
          return null;
                }
                else {
                  repository = null;
                  String s = loadErrors.toString();
                  loadErrors.delete(0, loadErrors.length());
                  loadErrors = null;
                  return s;
                }
        } catch (ParseException e1) {
          repository = null;
          String s = loadErrors.toString();
          loadErrors.delete(0, loadErrors.length());
          loadErrors = null;
          return s;
        }
  }

  final public void Start() throws ParseException {
    jj_consume_token(6);
    label_1:
    while (true) {
      switch ((jj_ntk==-1)?jj_ntk():jj_ntk) {
      case 9:
      case 10:
      case 13:
      case 15:
      case 16:
        ;
        break;
      default:
        jj_la1[0] = jj_gen;
        break label_1;
      }
      MetamodelOperation();
      jj_consume_token(7);
    }
    jj_consume_token(8);
    jj_consume_token(0);
  }

  final public void MetamodelOperation() throws ParseException {
    switch ((jj_ntk==-1)?jj_ntk():jj_ntk) {
    case 9:
      Class();
      break;
    case 10:
      Attr();
      break;
    case 13:
      Rel();
      break;
    case 15:
    case 16:
      Assoc();
      break;
    default:
      jj_la1[1] = jj_gen;
      jj_consume_token(-1);
      throw new ParseException();
    }
  }

  final public void Class() throws ParseException {
    jj_consume_token(9);
    jj_consume_token(NAME);
          if (!onlyCheckSyntax)
          {
                  if (MMDParser.debug)
                    System.out.println(" >>adding class `" + token.image + "'...");
                  long r = repository.findClass(removeQuotes(token.image));
                  if (r == 0) {
                        r = repository.createClass(removeQuotes(token.image));
                        if (r == 0)
                      loadErrors.append("Could not add class `" + token.image + ".\u005cn");
                  }

                  if (r != 0)
                    repository.freeReference(r);
          }
  }

  final public void Attr() throws ParseException {
  String className;
  String attrName;
  String attrType;
    jj_consume_token(10);
    jj_consume_token(NAME);
                className=removeQuotes(token.image);
    jj_consume_token(11);
    jj_consume_token(NAME);
                attrName=removeQuotes(token.image);
    jj_consume_token(12);
    jj_consume_token(NAME);
                attrType=removeQuotes(token.image);
          if (!onlyCheckSyntax)
          {
                  if (MMDParser.debug)
                    System.out.println(">>adding "+attrType+" attr "+attrName+" of "+className+"...");
                  long c = repository.findClass(className);
                  if (c==0)
                      c = repository.createClass(className);
                  if (c != 0) {
                          long t = repository.findPrimitiveDataType(attrType);
                          if (t != 0) {

                                  long r = repository.findAttribute(c, attrName);
                                  if (r == 0) {
                                    r = repository.createAttribute(c, attrName, t);
                                    if (r == 0)
                                      loadErrors.append("Could not add attribute `"+attrName+"' of type `"+attrType+"' for class `"+className+"'.\u005cn");
                                    else
                                      repository.freeReference(r);
                                  }
                                  else {
                                    long rt = repository.getAttributeType(r);
                                    if (rt != t) {
                                      loadErrors.append("Attribute `"+attrName+"' of type `"+attrType+"' for class `"+className
                                        +"' already exists, but has a different type ("+repository.getPrimitiveDataTypeName(rt)+" instead of "+repository.getPrimitiveDataTypeName(t)+").\u005cn");
                                      if (rt != 0)
                                        repository.freeReference(rt);
                                    }
                                    repository.freeReference(r);
                                  }
                                  repository.freeReference(t);
                          }
                          else {
                            loadErrors.append("Could not add attribute `"+attrName+"' of type `"+attrType+"' for class `"+className+"' since the type was not found.\u005cn");
                          }
                          repository.freeReference(c);
                 }
                 else {
                   loadErrors.append("Could not add attribute `"+attrName+"' of type `"+attrType+"' for class `"+className+"' since the class was not found.\u005cn");
                 }
          }
  }

  final public void Rel() throws ParseException {
        String subClassName;
        String superClassName;
    jj_consume_token(13);
    jj_consume_token(NAME);
                subClassName=removeQuotes(token.image);
    jj_consume_token(11);
    jj_consume_token(14);
    jj_consume_token(11);
    jj_consume_token(NAME);
                superClassName=removeQuotes(token.image);
          if (!onlyCheckSyntax)
          {
        if (MMDParser.debug)
                  System.out.println(">>generalization "+subClassName+"->"+superClassName+"...");
            long subRef = repository.findClass(subClassName);
            if (subRef==0)
              subRef = repository.createClass(subClassName);
            long superRef = repository.findClass(superClassName);
            if (superRef==0)
              superRef = repository.createClass(superClassName);

            boolean found = false;
            long it = repository.getIteratorForDirectSuperClasses(subRef);
            if (it != 0) {
              long r = repository.resolveIteratorFirst(it);
              while (r != 0) {
                if (r == superRef)
                  found = true;
                repository.freeReference(r);
                if (found)
                  break;
                r = repository.resolveIteratorNext(it);
              }
              repository.freeIterator(it);
            }

            if (!found) {
              boolean result = repository.createGeneralization(subRef, superRef);
              if (!result)
                loadErrors.append("Could not create generalization: "+subClassName+"->"+superClassName+".\u005cn");
            }
            repository.freeReference(subRef);
            repository.freeReference(superRef);
          }
  }

  final public void Assoc() throws ParseException {
        boolean isComposition;
        String sourceClass;
        String sourceRole;
        String sourceCardinality;
        String targetClass;
        String targetRole;
        String targetCardinality;
        boolean sourceOrdered;
        boolean targetOrdered;
    switch ((jj_ntk==-1)?jj_ntk():jj_ntk) {
    case 15:
      jj_consume_token(15);
                   isComposition=false;
      break;
    case 16:
      jj_consume_token(16);
                   isComposition=true;
      break;
    default:
      jj_la1[2] = jj_gen;
      jj_consume_token(-1);
      throw new ParseException();
    }
    jj_consume_token(NAME);
                sourceClass=removeQuotes(token.image);
    jj_consume_token(11);
    switch ((jj_ntk==-1)?jj_ntk():jj_ntk) {
    case 17:
      jj_consume_token(17);
      break;
    default:
      jj_la1[3] = jj_gen;
      ;
    }
                       sourceOrdered = token.image.equals("{ordered}");
    jj_consume_token(18);
    Cardinality();
                       sourceCardinality=token.image;
    jj_consume_token(19);
    jj_consume_token(NAME);
                sourceRole=removeQuotes(token.image);
    jj_consume_token(20);
    jj_consume_token(NAME);
                targetRole=removeQuotes(token.image);
    jj_consume_token(18);
    Cardinality();
                       targetCardinality=token.image;
    jj_consume_token(19);
    switch ((jj_ntk==-1)?jj_ntk():jj_ntk) {
    case 17:
      jj_consume_token(17);
      break;
    default:
      jj_la1[4] = jj_gen;
      ;
    }
                       targetOrdered = token.image.equals("{ordered}");
    jj_consume_token(11);
    jj_consume_token(NAME);
                targetClass=removeQuotes(token.image);
          if (!onlyCheckSyntax)
          {
        if (MMDParser.debug) {
              if (isComposition)
                System.out.print(">>compos ");
              else
                System.out.print(">>assoc ");
              System.out.println(sourceClass+" "+sourceCardinality+" "+sourceRole+" "+sourceOrdered+" <-> "+
                                         targetClass+" "+targetCardinality+" "+targetRole+" "+targetOrdered+"...");
        }

                String msg = "Could not create ";
                if (isComposition)
                  msg += "composition ";
                else
                  msg += "association ";
            msg += "between "+sourceClass+"["+sourceCardinality+"]("+sourceRole+") and "+
                                     targetClass+"["+targetCardinality+"]("+targetRole+").";

            long sourceRef = repository.findClass(sourceClass);
            if (sourceRef == 0)
                sourceRef = repository.createClass(sourceClass);
            if (sourceRef != 0) {
                    long targetRef = repository.findClass(targetClass);
                    if (targetRef == 0)
                        targetRef = repository.createClass(targetClass);
                    if (targetRef != 0) {
                            long r1 = repository.findAssociationEnd(sourceRef, targetRole);
                            if (r1 != 0) {
                                long r2 = repository.getInverseAssociationEnd(r1);
                                if (r2 != 0) {
                                  if (!sourceRole.equals(repository.getRoleName(r2))) {
                                    loadErrors.append(msg+" The association already exists, but has another inverse role.\u005cn");
                                  }
                                  repository.freeReference(r2);
                                }
                                else {
                                  loadErrors.append(msg+" The association already exists and is unidirectional.\u005cn");
                                }
                                repository.freeReference(r1);
                            }
                            else
                            {
                                    long assocRef = repository.createAssociation(sourceRef, targetRef,
                                                sourceRole, targetRole, isComposition);
                                        if (assocRef != 0)
                                  repository.freeReference(assocRef);
                                else
                                  loadErrors.append(msg+"\u005cn");
                                }
                        repository.freeReference(targetRef);
                        }
                        else {
                          loadErrors.append(msg+" The target class not found.\u005cn");
                        }
                repository.freeReference(sourceRef);
        }
        else
          loadErrors.append(msg+" The source class not found.\u005cn");

          }
  }

  final public void Cardinality() throws ParseException {
    switch ((jj_ntk==-1)?jj_ntk():jj_ntk) {
    case 21:
      jj_consume_token(21);
      break;
    case 22:
      jj_consume_token(22);
      break;
    case 23:
      jj_consume_token(23);
      break;
    case 24:
      jj_consume_token(24);
      break;
    case 25:
      jj_consume_token(25);
      break;
    case 26:
      jj_consume_token(26);
      break;
    default:
      jj_la1[5] = jj_gen;
      jj_consume_token(-1);
      throw new ParseException();
    }
  }

  /** Generated Token Manager. */
  public MMDParserTokenManager token_source;
  SimpleCharStream jj_input_stream;
  /** Current token. */
  public Token token;
  /** Next token. */
  public Token jj_nt;
  private int jj_ntk;
  private int jj_gen;
  final private int[] jj_la1 = new int[6];
  static private int[] jj_la1_0;
  static {
      jj_la1_init_0();
   }
   private static void jj_la1_init_0() {
      jj_la1_0 = new int[] {0x1a600,0x1a600,0x18000,0x20000,0x20000,0x7e00000,};
   }

  /** Constructor with InputStream. */
  public MMDParser(java.io.InputStream stream) {
     this(stream, null);
  }
  /** Constructor with InputStream and supplied encoding */
  public MMDParser(java.io.InputStream stream, String encoding) {
    try { jj_input_stream = new SimpleCharStream(stream, encoding, 1, 1); } catch(java.io.UnsupportedEncodingException e) { throw new RuntimeException(e); }
    token_source = new MMDParserTokenManager(jj_input_stream);
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 6; i++) jj_la1[i] = -1;
  }

  /** Reinitialise. */
  public void ReInit(java.io.InputStream stream) {
     ReInit(stream, null);
  }
  /** Reinitialise. */
  public void ReInit(java.io.InputStream stream, String encoding) {
    try { jj_input_stream.ReInit(stream, encoding, 1, 1); } catch(java.io.UnsupportedEncodingException e) { throw new RuntimeException(e); }
    token_source.ReInit(jj_input_stream);
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 6; i++) jj_la1[i] = -1;
  }

  /** Constructor. */
  public MMDParser(java.io.Reader stream) {
    jj_input_stream = new SimpleCharStream(stream, 1, 1);
    token_source = new MMDParserTokenManager(jj_input_stream);
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 6; i++) jj_la1[i] = -1;
  }

  /** Reinitialise. */
  public void ReInit(java.io.Reader stream) {
    jj_input_stream.ReInit(stream, 1, 1);
    token_source.ReInit(jj_input_stream);
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 6; i++) jj_la1[i] = -1;
  }

  /** Constructor with generated Token Manager. */
  public MMDParser(MMDParserTokenManager tm) {
    token_source = tm;
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 6; i++) jj_la1[i] = -1;
  }

  /** Reinitialise. */
  public void ReInit(MMDParserTokenManager tm) {
    token_source = tm;
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 6; i++) jj_la1[i] = -1;
  }

  private Token jj_consume_token(int kind) throws ParseException {
    Token oldToken;
    if ((oldToken = token).next != null) token = token.next;
    else token = token.next = token_source.getNextToken();
    jj_ntk = -1;
    if (token.kind == kind) {
      jj_gen++;
      return token;
    }
    token = oldToken;
    jj_kind = kind;
    throw generateParseException();
  }


/** Get the next Token. */
  final public Token getNextToken() {
    if (token.next != null) token = token.next;
    else token = token.next = token_source.getNextToken();
    jj_ntk = -1;
    jj_gen++;
    return token;
  }

/** Get the specific Token. */
  final public Token getToken(int index) {
    Token t = token;
    for (int i = 0; i < index; i++) {
      if (t.next != null) t = t.next;
      else t = t.next = token_source.getNextToken();
    }
    return t;
  }

  private int jj_ntk() {
    if ((jj_nt=token.next) == null)
      return (jj_ntk = (token.next=token_source.getNextToken()).kind);
    else
      return (jj_ntk = jj_nt.kind);
  }

  private java.util.List<int[]> jj_expentries = new java.util.ArrayList<int[]>();
  private int[] jj_expentry;
  private int jj_kind = -1;

  /** Generate ParseException. */
  public ParseException generateParseException() {
    jj_expentries.clear();
    boolean[] la1tokens = new boolean[28];
    if (jj_kind >= 0) {
      la1tokens[jj_kind] = true;
      jj_kind = -1;
    }
    for (int i = 0; i < 6; i++) {
      if (jj_la1[i] == jj_gen) {
        for (int j = 0; j < 32; j++) {
          if ((jj_la1_0[i] & (1<<j)) != 0) {
            la1tokens[j] = true;
          }
        }
      }
    }
    for (int i = 0; i < 28; i++) {
      if (la1tokens[i]) {
        jj_expentry = new int[1];
        jj_expentry[0] = i;
        jj_expentries.add(jj_expentry);
      }
    }
    int[][] exptokseq = new int[jj_expentries.size()][];
    for (int i = 0; i < jj_expentries.size(); i++) {
      exptokseq[i] = jj_expentries.get(i);
    }
    return new ParseException(token, exptokseq, tokenImage);
  }

  /** Enable tracing. */
  final public void enable_tracing() {
  }

  /** Disable tracing. */
  final public void disable_tracing() {
  }

}
